using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using DDebugger.Breakpoints;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;

namespace DebuggerTest
{
	// Copyright (c) AlphaSierraPapa for the SharpDevelop Team
	// This code is distributed under the GNU LGPL

	// Edited by A. Bothe

	/// <summary>
	/// Handles the text markers for a code editor.
	/// </summary>
	public sealed class TextMarkerService : DocumentColorizingTransformer, IBackgroundRenderer
	{
		public readonly TextEditor Editor;
		TextSegmentCollection<LineMarker> markers;

		/// <summary>
		/// Initializes the marker service and registers itself with the codeEditor
		/// </summary>
		/// <param name="codeEditor"></param>
		public TextMarkerService(TextEditor codeEditor)
		{
			if (codeEditor == null)
				throw new ArgumentNullException("codeEditor");
			this.Editor = codeEditor;
			markers = new TextSegmentCollection<LineMarker>(Editor.Document);

			var tv = Editor.TextArea.TextView;
			tv.Services.AddService(typeof(TextMarkerService), this);
			tv.LineTransformers.Add(this);
			tv.BackgroundRenderers.Add(this);
		}

		#region ITextMarkerService
		public void Add(LineMarker m)
		{
			if (markers.Contains(m))
				return;

			markers.Add(m);
		}

		public IEnumerable<LineMarker> GetMarkersAtOffset(int offset)
		{
			return markers.FindSegmentsContaining(offset);
		}

		public IEnumerable<LineMarker> TextMarkers
		{
			get { return markers; }
		}

		public void RemoveAll(Predicate<LineMarker> predicate)
		{
			if (predicate == null)
				throw new ArgumentNullException("predicate");
			foreach (LineMarker m in markers.ToArray())
			{
				if (predicate(m))
					Remove(m);
			}
		}

		public void RemoveAll()
		{
			foreach (LineMarker m in markers.ToArray())
					Remove(m);
		}

		public void Remove(LineMarker marker)
		{
			if (marker == null)
				throw new ArgumentNullException("marker");
			var m = marker as LineMarker;
			if (markers.Remove(m))
			{
				Redraw(m);
			}
		}

		/// <summary>
		/// Redraws the specified text segment.
		/// </summary>
		internal void Redraw(ISegment segment)
		{
			Editor.TextArea.TextView.Redraw(segment, DispatcherPriority.Normal);
		}
		#endregion

		#region DocumentColorizingTransformer
		protected override void ColorizeLine(DocumentLine line)
		{
			if (markers == null)
				return;
			int lineStart = line.Offset;
			int lineEnd = lineStart + line.Length;
			foreach (LineMarker marker in markers.FindOverlappingSegments(lineStart, line.Length))
			{
				Brush foregroundBrush = null;
				if (marker.ForegroundColor != null)
				{
					foregroundBrush = new SolidColorBrush(marker.ForegroundColor.Value);
					foregroundBrush.Freeze();
				}
				ChangeLinePart(
					Math.Max(marker.StartOffset, lineStart),
					Math.Min(marker.EndOffset, lineEnd),
					element =>
					{
						if (foregroundBrush != null)
						{
							element.TextRunProperties.SetForegroundBrush(foregroundBrush);
						}
					}
				);
			}
		}
		#endregion

		#region IBackgroundRenderer
		public KnownLayer Layer
		{
			get
			{
				// draw behind selection
				return KnownLayer.Selection;
			}
		}

		public void Draw(TextView textView, DrawingContext drawingContext)
		{
			if (textView == null)
				throw new ArgumentNullException("textView");
			if (drawingContext == null)
				throw new ArgumentNullException("drawingContext");
			if (markers == null || !textView.VisualLinesValid)
				return;
			var visualLines = textView.VisualLines;
			if (visualLines.Count == 0)
				return;
			int viewStart = visualLines.First().FirstDocumentLine.Offset;
			int viewEnd = visualLines.Last().LastDocumentLine.Offset + visualLines.Last().LastDocumentLine.Length;

			foreach (LineMarker marker in markers.FindOverlappingSegments(viewStart, viewEnd - viewStart))
			{
				if (marker.BackgroundColor != null)
				{
					BackgroundGeometryBuilder geoBuilder = new BackgroundGeometryBuilder();
					geoBuilder.AlignToWholePixels = true;
					geoBuilder.CornerRadius = 3;
					geoBuilder.AddSegment(textView, marker);
					Geometry geometry = geoBuilder.CreateGeometry();
					if (geometry != null)
					{
						Color color = marker.BackgroundColor.Value;
						SolidColorBrush brush = new SolidColorBrush(color);
						brush.Freeze();
						drawingContext.DrawGeometry(brush, null, geometry);
					}
				}
				if (marker.MarkerType != TextMarkerType.None)
				{
					foreach (Rect r in BackgroundGeometryBuilder.GetRectsForSegment(textView, marker))
					{
						Point startPoint = r.BottomLeft;
						Point endPoint = r.BottomRight;

						Pen usedPen = new Pen(new SolidColorBrush(marker.MarkerColor), 1);
						usedPen.Freeze();
						switch (marker.MarkerType)
						{
							case TextMarkerType.Underlined:
								double offset = 2.5;

								int count = Math.Max((int)((endPoint.X - startPoint.X) / offset) + 1, 4);

								StreamGeometry geometry = new StreamGeometry();

								using (StreamGeometryContext ctx = geometry.Open())
								{
									ctx.BeginFigure(startPoint, false, false);
									ctx.PolyLineTo(CreatePoints(startPoint, endPoint, offset, count).ToArray(), true, false);
								}

								geometry.Freeze();

								drawingContext.DrawGeometry(Brushes.Transparent, usedPen, geometry);
								break;
						}
					}
				}
			}
		}

		IEnumerable<Point> CreatePoints(Point start, Point end, double offset, int count)
		{
			for (int i = 0; i < count; i++)
				yield return new Point(start.X + i * offset, start.Y - ((i + 1) % 2 == 0 ? offset : 0));
		}
		#endregion
	}

	public class LineMarker : TextSegment
	{
		public readonly TextMarkerService TextMarkerService;

		public LineMarker(TextMarkerService svc, TextDocument doc, int beginLine, int endLine)
		{
			TextMarkerService = svc;
			StartOffset = doc.GetOffset(beginLine, 0);
			Length = doc.GetLineByNumber(endLine).EndOffset - StartOffset;
		}
		/*
		public LineMarker(TextMarkerService svc, int offset, int length)
		{
			TextMarkerService = svc;
			StartOffset = offset;
			Length = length;
		}*/

		public object Tag { get; set; }

		public Color? BackgroundColor;
		public Color? ForegroundColor;
		public Color MarkerColor = Colors.Red;

		public TextMarkerType MarkerType = TextMarkerType.Underlined;

		public void Delete()
		{
			TextMarkerService.Remove(this);
		}

		public void Redraw()
		{
			TextMarkerService.Redraw(this);
		}

		public string ToolTip { get; set; }
	}

	public enum TextMarkerType
	{
		None,
		Underlined
	}

	public class DebugInfoAvailableMarker : LineMarker
	{
		public DebugInfoAvailableMarker(TextMarkerService markerStrategy, TextDocument doc, int line_begin, int line_end)
			: base(markerStrategy, doc, line_begin, line_end)
		{
			BackgroundColor = Colors.LightGreen;
			MarkerType = TextMarkerType.None;
		}
	}

	public class BreakpointMarker : LineMarker
	{
		public readonly Breakpoint Breakpoint;

		public BreakpointMarker(TextMarkerService markerStrategy, Breakpoint bp, TextDocument doc, int line_begin, int line_end)
			: base(markerStrategy, doc, line_begin, line_end)
		{
			this.Breakpoint = bp;
			MarkerType = TextMarkerType.None;
			BackgroundColor = Colors.Red;
		}
	}
}

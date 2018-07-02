using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace CardinalAppXamarin.Controls
{
    public class HexagonLayout : Layout<View>
    {
        Dictionary<Size, LayoutData> layoutDataCache = new Dictionary<Size, LayoutData>();

        public static readonly BindableProperty RadiusProperty = BindableProperty.Create(
                "Radius",
                typeof(double),
                typeof(HexagonLayout),
                10.0,
                propertyChanged: (bindable, oldvalue, newvalue) =>
                {
                    ((HexagonLayout)bindable).InvalidateLayout();
                });

        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        public static readonly BindableProperty PointyTopProperty = BindableProperty.Create(
                "PointyTop",
                typeof(bool),
                typeof(HexagonLayout),
                true,
                propertyChanged: (bindable, oldvalue, newvalue) =>
                {
                    ((HexagonLayout)bindable).InvalidateLayout();
                });

        public bool PointyTop
        {
            get { return (bool)GetValue(PointyTopProperty); }
            set { SetValue(PointyTopProperty, value); }
        }

        public static readonly BindableProperty ColumnSpacingProperty = BindableProperty.Create(
              "ColumnSpacing",
              typeof(double),
              typeof(HexagonLayout),
              5.0,
              propertyChanged: (bindable, oldvalue, newvalue) =>
              {
                  ((HexagonLayout)bindable).InvalidateLayout();
              });

        public double ColumnSpacing
        {
            get { return (double)GetValue(ColumnSpacingProperty); }
            set { SetValue(ColumnSpacingProperty, value); }
        }

        public static readonly BindableProperty RowSpacingProperty = BindableProperty.Create(
              "RowSpacing",
              typeof(double),
              typeof(HexagonLayout),
              5.0,
              propertyChanged: (bindable, oldvalue, newvalue) =>
              {
                  ((HexagonLayout)bindable).InvalidateLayout();
              });

        public double RowSpacing
        {
            get { return (double)GetValue(RowSpacingProperty); }
            set { SetValue(RowSpacingProperty, value); }
        }

        private double HexagonHeight
        {
            get
            {
                if (PointyTop)
                {
                    return Radius * 2;
                }
                else
                {
                    return Radius * Math.Cos(Math.PI / 180 * 30) * 2;
                }
            }
        }

        private double HexagonWidth
        {
            get
            {
                if(PointyTop)
                {
                    return Radius * Math.Cos(Math.PI / 180 * 30) * 2;
                }
                else
                {
                    return Radius * 2;
                }
            }
        }

        private LayoutData GetLayoutData(double width, double height)
        {
            Size size = new Size(width, height);

            // Check if cached information is available.
            if (layoutDataCache.ContainsKey(size))
            {
                return layoutDataCache[size];
            }

            int visibleChildCount = 0;
            Size maxChildSize = new Size();
            int rows = 0;
            int columns = 0;
            LayoutData layoutData = new LayoutData();

            /*
            //Enumerate through all the children.
            foreach (View child in Children)
            {
                // Skip invisible children.
                if (!child.IsVisible)
                    continue;

                // Count the visible children.
                visibleChildCount++;

                // Get the child's requested size.
                SizeRequest childSizeRequest = child.Measure(Double.PositiveInfinity, Double.PositiveInfinity);

                // Accumulate the maximum child size.
                maxChildSize.Width = Math.Max(maxChildSize.Width, childSizeRequest.Request.Width);
                maxChildSize.Height = Math.Max(maxChildSize.Height, childSizeRequest.Request.Height);
            }
            */

            // For now force children into a size determined by Radius property
            maxChildSize.Width = HexagonWidth;
            maxChildSize.Height = HexagonHeight;
            visibleChildCount = Children.Where(c => c.IsVisible).Count();

            if (visibleChildCount != 0)
            {
                // Calculate the number of rows and columns.
                if (Double.IsPositiveInfinity(width))
                {
                    columns = visibleChildCount;
                    rows = 1;
                }
                else
                {
                    columns = (int)((width + ColumnSpacing) / (maxChildSize.Width + ColumnSpacing));
                    columns = Math.Max(1, columns);
                    rows = (visibleChildCount + columns - 1) / columns;
                }

                /*
                // Now maximize the cell size based on the layout size.
                Size cellSize = new Size();

                if (Double.IsPositiveInfinity(width))
                    cellSize.Width = maxChildSize.Width;
                else
                    cellSize.Width = (width - ColumnSpacing * (columns - 1)) / columns;

                if (Double.IsPositiveInfinity(height))
                    cellSize.Height = maxChildSize.Height;
                else
                    cellSize.Height = (height - RowSpacing * (rows - 1)) / rows;
                */

                // Keep the cells pre-defined size based off of radius
                Size cellSize = new Size(HexagonWidth,
                                         HexagonHeight);

                layoutData = new LayoutData(visibleChildCount, cellSize, rows, columns);
            }

            layoutDataCache.Add(size, layoutData);
            return layoutData;
        }

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            LayoutData layoutData = GetLayoutData(widthConstraint, heightConstraint);
            if (layoutData.VisibleChildCount == 0)
            {
                return new SizeRequest();
            }

            Size totalSize = new Size(layoutData.CellSize.Width * layoutData.Columns + ColumnSpacing * (layoutData.Columns - 1),
                                      layoutData.CellSize.Height * layoutData.Rows + RowSpacing * (layoutData.Rows - 1));
            return new SizeRequest(totalSize);
        }

        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            LayoutData layoutData = GetLayoutData(width, height);

            if (layoutData.VisibleChildCount == 0)
            {
                return;
            }

            double halfWidth = HexagonWidth / 2;
            double quarterWidth = halfWidth / 2;
            double halfHeight = HexagonHeight / 2;
            double quarterHeight = halfHeight / 2;

            double xChild = x;
            double yChild = y;

            if (PointyTop)
            {
                //Start off with first element placed at 0.5w-1.5w x 0.5h-1.5h
                xChild += halfWidth;
                yChild += halfHeight;
            }
            else
            {
                //Start off with first element placed at 0.75w-1.75w x 0h-1h
                xChild += halfWidth + quarterWidth;
            }
            int row = 0;
            int column = 0;
            bool even = true;

            foreach (View child in Children)
            {
                if (!child.IsVisible)
                {
                    continue;
                }
                LayoutChildIntoBoundingRegion(child, new Rectangle(new Point(xChild, yChild), layoutData.CellSize));
                if (PointyTop)
                {
                    if (++column == layoutData.Columns)
                    {
                        column = 0;
                        row++;
                        even = row % 2 == 0;
                        if (even)
                        {
                            //Shift even rows to the right by .5
                            xChild = x + halfHeight;
                        }
                        else
                        {
                            xChild = x;
                        }
                        yChild += RowSpacing + layoutData.CellSize.Height - quarterHeight;
                    }
                    else
                    {
                        xChild += ColumnSpacing + layoutData.CellSize.Width;
                    }
                }
                else
                {
                    //flat-top
                    if (++column == layoutData.Columns)
                    {
                        column = 0;
                        row++;
                        xChild = x + halfWidth + quarterWidth;
                        yChild += RowSpacing + layoutData.CellSize.Height;
                    }
                    else
                    {
                        xChild += ColumnSpacing + layoutData.CellSize.Width - quarterWidth;
                        even = column % 2 == 0;
                        if (even)
                        {
                            //if added .5h, remove it for even columns to shift back down
                            if (column > 0)
                            {
                                yChild -= halfHeight;
                            }
                        }
                        else
                        {
                            //shift odd-columns up by .5h
                            yChild += halfHeight;
                        }
                    }
                }
            }
        }

        protected override void InvalidateLayout()
        {
            //to bypass this override the ShouldInvalidateOnChildAdded() and ShouldInvalidateOnChildRemoved() methods
            base.InvalidateLayout();
            layoutDataCache.Clear();
        }

        protected override void OnChildMeasureInvalidated()
        {
            base.OnChildMeasureInvalidated();
            layoutDataCache.Clear();
        }
    }
}

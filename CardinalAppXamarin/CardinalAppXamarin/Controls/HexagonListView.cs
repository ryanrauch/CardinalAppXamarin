﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace CardinalAppXamarin.Controls
{
    public class HexagonListView : Layout<View>
    {
        Dictionary<Size, LayoutData> layoutDataCache = new Dictionary<Size, LayoutData>();

        public static readonly BindableProperty RadiusProperty =
            BindableProperty.Create("Radius",
                                    typeof(double),
                                    typeof(HexagonListView),
                                    40.0,
                                    propertyChanged: (bindable, oldvalue, newvalue) =>
                                    {
                                        ((HexagonListView)bindable).InvalidateLayout();
                                    });

        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        public static readonly BindableProperty PointyTopProperty =
            BindableProperty.Create("PointyTop",
                                    typeof(bool),
                                    typeof(HexagonListView),
                                    true,
                                    propertyChanged: (bindable, oldvalue, newvalue) =>
                                    {
                                        ((HexagonListView)bindable).InvalidateLayout();
                                    });

        public bool PointyTop
        {
            get { return (bool)GetValue(PointyTopProperty); }
            set { SetValue(PointyTopProperty, value); }
        }

        public static readonly BindableProperty RowSpacingProperty = BindableProperty.Create(
              "RowSpacing",
              typeof(double),
              typeof(HexagonListView),
              5.0,
              propertyChanged: (bindable, oldvalue, newvalue) =>
              {
                  ((HexagonListView)bindable).InvalidateLayout();
              });

        public double RowSpacing
        {
            get { return (double)GetValue(RowSpacingProperty); }
            set { SetValue(RowSpacingProperty, value); }
        }

        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(
                nameof(ItemsSource),
                typeof(IEnumerable),
                typeof(HexagonListView),
                null,
                defaultBindingMode: BindingMode.OneWay,
                propertyChanged: ItemsChanged
            );

        public IList ItemsSource
        {
            get { return (IList)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly BindableProperty ItemTemplateProperty =
            BindableProperty.Create(
                nameof(ItemTemplate),
                typeof(DataTemplate),
                typeof(HexagonListView),
                default(DataTemplate),
                propertyChanged: (bindable, oldValue, newValue) => {
                    var control = (HexagonListView)bindable;
                    //when to occur propertychanged earlier ItemsSource than ItemTemplate, raise ItemsChanged manually
                    if (newValue != null && control.ItemsSource != null && !control.doneItemSourceChanged)
                    {
                        ItemsChanged(bindable, null, control.ItemsSource);
                    }
                }
            );

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        private static void OnItemsSourceChanged(BindableObject pObj, object pOldVal, object pNewVal)
        {
            var layout = pObj as HexagonListView;

            if (layout != null && layout.ItemTemplate != null)
                layout.BuildLayout();
        }

        private void BuildLayout()
        {
            Children.Clear();
            if (ItemsSource == null)
            {
                return;
            }
            foreach (var item in ItemsSource)
            {
                var view = (View)ItemTemplate.CreateContent();
                view.BindingContext = item;
                Children.Add(view);
            }
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
                if (PointyTop)
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

            // Force children into a size (height) determined by Radius property
            maxChildSize.Width = width;
            maxChildSize.Height = HexagonHeight;
            visibleChildCount = Children.Where(c => c.IsVisible).Count();
            columns = 1;
            rows = (visibleChildCount + columns - 1) / columns;
            Size cellSize = new Size(width,
                                     HexagonHeight);

            layoutData = new LayoutData(visibleChildCount, cellSize, rows, columns);

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
            if (PointyTop)
            {
                return new SizeRequest(new Size(layoutData.CellSize.Width,
                                                layoutData.CellSize.Height * layoutData.Rows + RowSpacing * (layoutData.Rows - 1) - layoutData.CellSize.Height/4 * (layoutData.Rows - 1)));
            }
            Size totalSize = new Size(layoutData.CellSize.Width,
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

            int row = 0;
            bool odd = true;

            foreach (View child in Children)
            {
                if (!child.IsVisible)
                {
                    continue;
                }
                LayoutChildIntoBoundingRegion(child, new Rectangle(new Point(xChild, yChild), layoutData.CellSize));

                row++;
                odd = row % 2 != 0;
                if (odd)
                {
                    //Shift odd rows to the right by .5
                    xChild = x + halfWidth;
                }
                else
                {
                    xChild = x;
                }
                yChild += RowSpacing + layoutData.CellSize.Height - quarterHeight + RowSpacing;
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

        //start of copied section
        private bool doneItemSourceChanged = false;

        private static void ItemsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (HexagonListView)bindable;
            // when to occur propertychanged earlier ItemsSource than ItemTemplate, do nothing.
            if (control.ItemTemplate == null)
            {
                control.doneItemSourceChanged = false;
                return;
            }

            control.doneItemSourceChanged = true;

            IEnumerable newValueAsEnumerable;
            try
            {
                newValueAsEnumerable = newValue as IEnumerable;
            }
            catch (Exception e)
            {
                throw e;
            }

            var oldObservableCollection = oldValue as INotifyCollectionChanged;

            if (oldObservableCollection != null)
            {
                oldObservableCollection.CollectionChanged -= control.OnItemsSourceCollectionChanged;
            }

            var newObservableCollection = newValue as INotifyCollectionChanged;

            if (newObservableCollection != null)
            {
                newObservableCollection.CollectionChanged += control.OnItemsSourceCollectionChanged;
            }

            control.Children.Clear();

            if (newValueAsEnumerable != null)
            {
                foreach (var item in newValueAsEnumerable)
                {
                    var view = CreateChildViewFor(control.ItemTemplate, item, control);

                    control.Children.Add(view);
                }
            }

            control.UpdateChildrenLayout();
            control.InvalidateLayout();
        }

        private void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Replace)
            {

                this.Children.RemoveAt(e.OldStartingIndex);

                var item = e.NewItems[e.NewStartingIndex];
                var view = CreateChildViewFor(this.ItemTemplate, item, this);

                this.Children.Insert(e.NewStartingIndex, view);
            }

            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (e.NewItems != null)
                {
                    for (var i = 0; i < e.NewItems.Count; ++i)
                    {
                        var item = e.NewItems[i];
                        var view = CreateChildViewFor(this.ItemTemplate, item, this);

                        this.Children.Insert(i + e.NewStartingIndex, view);
                    }
                }
            }

            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                if (e.OldItems != null)
                {
                    this.Children.RemoveAt(e.OldStartingIndex);
                }
            }

            else if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                this.Children.Clear();
            }

            else
            {
                return;
            }

        }

        private View CreateChildViewFor(object item)
        {
            this.ItemTemplate.SetValue(BindableObject.BindingContextProperty, item);
            return (View)this.ItemTemplate.CreateContent();
        }

        private static View CreateChildViewFor(DataTemplate template, object item, BindableObject container)
        {
            var selector = template as DataTemplateSelector;

            if (selector != null)
            {
                template = selector.SelectTemplate(item, container);
            }
            //Binding context
            template.SetValue(BindableObject.BindingContextProperty, item);
            return (View)template.CreateContent();
        }
    }
}

namespace Wpf_Arrows_Package
{
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// ����ͷ�ı���������
    /// </summary>
    public class ArrowBezierCurve : ArrowBase
    {
        #region Fields

        #region DependencyProperty

        /// <summary>
        /// ���Ƶ�1
        /// </summary>
        public static readonly DependencyProperty ControlPoint1Property = DependencyProperty.Register(
            "ControlPoint1",
            typeof(Point),
            typeof(ArrowBezierCurve),
            new FrameworkPropertyMetadata(
                new Point(),
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// ���Ƶ�2
        /// </summary>
        public static readonly DependencyProperty ControlPoint2Property = DependencyProperty.Register(
            "ControlPoint2", typeof(Point), typeof(ArrowBezierCurve), new FrameworkPropertyMetadata(new Point(), FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// ������
        /// </summary>
        public static readonly DependencyProperty EndPointProperty = DependencyProperty.Register(
            "EndPoint", typeof(Point), typeof(ArrowBezierCurve), new FrameworkPropertyMetadata(new Point(), FrameworkPropertyMetadataOptions.AffectsMeasure));

        #endregion DependencyProperty

        /// <summary>
        /// ����������
        /// </summary>
        private readonly BezierSegment bezierSegment = new BezierSegment();

        #endregion Fields

        #region Properties

        /// <summary>
        /// ���Ƶ�1
        /// </summary>
        public Point ControlPoint1
        {
            get { return (Point)this.GetValue(ControlPoint1Property); }
            set { this.SetValue(ControlPoint1Property, value); }
        }

        /// <summary>
        /// ���Ƶ�2
        /// </summary>
        public Point ControlPoint2
        {
            get { return (Point)this.GetValue(ControlPoint2Property); }
            set { this.SetValue(ControlPoint2Property, value); }
        }

        /// <summary>
        /// ������
        /// </summary>
        public Point EndPoint
        {
            get { return (Point)this.GetValue(EndPointProperty); }
            set { this.SetValue(EndPointProperty, value); }
        }

        #endregion Properties

        #region Protected Methods

        /// <summary>
        /// ���Figure
        /// </summary>
        /// <returns>PathSegment����</returns>
        protected override PathSegmentCollection FillFigure()
        {
            this.bezierSegment.Point1 = this.ControlPoint1;
            this.bezierSegment.Point2 = this.ControlPoint2;
            this.bezierSegment.Point3 = this.EndPoint;

            return new PathSegmentCollection
            {
                this.bezierSegment
            };
        }

        /// <summary>
        /// ��ȡ��ʼ��ͷ���Ľ�����
        /// </summary>
        /// <returns>��ʼ��ͷ���Ľ�����</returns>
        protected override Point GetStartArrowEndPoint()
        {
            return this.ControlPoint1;
        }

        /// <summary>
        /// ��ȡ������ͷ���Ŀ�ʼ��
        /// </summary>
        /// <returns>������ͷ���Ŀ�ʼ��</returns>
        protected override Point GetEndArrowStartPoint()
        {
            return this.ControlPoint2;
        }

        /// <summary>
        /// ��ȡ������ͷ���Ľ�����
        /// </summary>
        /// <returns>������ͷ���Ľ�����</returns>
        protected override Point GetEndArrowEndPoint()
        {
            return this.EndPoint;
        }

        #endregion  Protected Methods
    }
}

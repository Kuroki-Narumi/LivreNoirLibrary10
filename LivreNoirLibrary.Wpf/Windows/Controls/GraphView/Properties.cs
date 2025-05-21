using System.Windows.Media;
using static LivreNoirLibrary.Media.MediaUtils;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Windows.Controls
{
    public partial class GraphView
    {
        public const bool DefaultIsNodeNameVisible = true;
        public const double DefaultNodeRadius = 20;
        public const double DefaultNodeStrokeThickness = 1;
        public static readonly SolidColorBrush DefaultNodeFill = GetBrush(255, 255, 255, 255);
        public static readonly SolidColorBrush DefaultNodeStroke = GetBrush(255, 0, 0, 0);
        public static readonly SolidColorBrush DefaultSelectedNodeFill = GetBrush(255, 192, 192, 255);
        public static readonly SolidColorBrush DefaultSelectedNodeStroke = GetBrush(255, 0, 0, 127);
        public static readonly SolidColorBrush DefaultConnectedNodeFill = GetBrush(255, 192, 255, 192);
        public static readonly SolidColorBrush DefaultConnectedNodeStroke = GetBrush(255, 0, 127, 0);

        public const bool DefaultIsEdgeNameVisible = false;
        public const double DefaultEdgeKnobRadius = 10;
        public static readonly SolidColorBrush DefaultEdgeBrush = GetBrush(95, 0, 0, 0);
        public const double DefaultEdgeThickness = 2;
        public static readonly SolidColorBrush DefaultSelectedEdgeBrush = GetBrush(191, 0, 127, 255);
        public const double DefaultSelectedEdgeThickness = 4;

        public const bool DefaultIsInteractive = true;
        public const double DefaultGrid = DefaultNodeRadius * 2;
        public const double MoveThreshold = 9.0;

        protected virtual void OnSourceChanged(Graph? oldSource, Graph? newSource)
        {
            if (oldSource is not null)
            {
                oldSource.NodeChanged -= OnNodeChanged;
                oldSource.EdgeChanged -= OnEdgeChanged;
            }
            if (newSource is not null)
            {
                newSource.NodeChanged += OnNodeChanged;
                newSource.EdgeChanged += OnEdgeChanged;
            }
            _source = newSource;
            ReserveRefresh();
        }

        protected virtual void OnSelectedNodeChanged(GraphNode? value)
        {
            _selected_viewNode = GetViewNode(value);
            UpdateSelection();
        }

        [DependencyProperty(BindsTwoWayByDefault = true, AffectsRender = true)]
        protected double _offsetX;
        [DependencyProperty(BindsTwoWayByDefault = true, AffectsRender = true)]
        protected double _offsetY;
        [DependencyProperty(BindsTwoWayByDefault = true, AffectsRender = true)]
        private Graph? _source;
        [DependencyProperty(BindsTwoWayByDefault = true, AffectsRender = true)]
        private GraphNode? _selectedNode;
        [DependencyProperty(AffectsRender = true)]
        private bool _isNodeNameVisible = DefaultIsNodeNameVisible;
        [DependencyProperty(AffectsRender = true)]
        private double _nodeRadius = DefaultNodeRadius;
        [DependencyProperty(AffectsRender = true)]
        private Brush? _nodeFill = DefaultNodeFill;
        [DependencyProperty(AffectsRender = true)]
        private Brush? _nodeStroke = DefaultNodeStroke;
        [DependencyProperty(AffectsRender = true)]
        private double _nodeStrokeThickness = DefaultNodeStrokeThickness;
        [DependencyProperty(AffectsRender = true)]
        private Brush? _selectedNodeFill = DefaultSelectedNodeFill;
        [DependencyProperty(AffectsRender = true)]
        private Brush? _selectedNodeStroke = DefaultSelectedNodeStroke;
        [DependencyProperty(AffectsRender = true)]
        private Brush? _connectedNodeFill = DefaultConnectedNodeFill;
        [DependencyProperty(AffectsRender = true)]
        private Brush? _connectedNodeStroke = DefaultConnectedNodeStroke;
        [DependencyProperty(AffectsRender = true)]
        private bool _isEdgeNameVisible = DefaultIsEdgeNameVisible;
        [DependencyProperty(AffectsRender = true)]
        private double _edgeNameFontSize = double.NaN;
        [DependencyProperty(AffectsRender = true)]
        private double _edgeKnobRadius = DefaultEdgeKnobRadius;
        [DependencyProperty(AffectsRender = true)]
        private Brush? _edgeBrush = DefaultEdgeBrush;
        [DependencyProperty(AffectsRender = true)]
        private double _edgeThickness = DefaultEdgeThickness;
        [DependencyProperty(AffectsRender = true)]
        private Brush? _selectedEdgeBrush = DefaultSelectedEdgeBrush;
        [DependencyProperty(AffectsRender = true)]
        private double _selectedEdgeThickness = DefaultSelectedEdgeThickness;
        [DependencyProperty]
        private bool _isInteractive = DefaultIsInteractive;
        [DependencyProperty]
        private double _verticalGrid = DefaultGrid;
        [DependencyProperty]
        private double _horizontalGrid = DefaultGrid;

        public double NodeDiameter { get => _nodeRadius * 2; set => NodeRadius = value / 2; }
    }
}

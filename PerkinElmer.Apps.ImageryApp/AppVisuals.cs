using Spotfire.Dxp.Application;
using Spotfire.Dxp.Application.Visuals;
using Spotfire.Dxp.Data;
using Spotfire.Dxp.Data.Formatters;
using System.Linq;
using PerkinElmer.Signals.Analytics.Components;
using System.Collections.Generic;
using System;
using System.Drawing;


namespace PerkinElmer.Apps.ImageryApp
{
    public partial class App : BaseComponentsApp
    {
        [VisualizationName("imageryBarChartViz")]
        public Visual CreateBarChartVisualization()
        {
            var barPlot = Page.Visuals.AddNew<BarChart>();
            barPlot.AutoConfigure();

            barPlot.Title = "% of Patients per Cut off Value (only for % cells)";

            barPlot.Data.DataTableReference = GetTableFromParameter("cutOffTable");

            List<String> myvalues = new List<String>() { ">5", ">10", ">20", ">30", ">40", ">50" };
            barPlot.Data.DataTableReference.Columns["variable"].Properties.SetCustomSortOrder(myvalues);

            barPlot.XAxis.Expression = "<[variable]>";
            barPlot.YAxis.Expression = "UniqueCount([value]) / Max([PatientsPerGroup]) as [% of patients]";
            barPlot.Trellis.PageAxis.Expression = "<[Marker]>";
            barPlot.ColorAxis.DefaultColor = Color.Blue;
            //YAxis format to percentage
            NumberFormatter nf = (NumberFormatter)DataType.Real.CreateLocalizedFormatter();
            nf.Category = NumberFormatCategory.Percentage;
            nf.DecimalDigits = 0;
            barPlot.YAxis.Scale.Formatting.RealFormatter = nf;
            barPlot.Data.WhereClauseExpression = "[Measurement, Test or Examination Detail] = \"% Cells\"";

            //Configure trellis font
            Font trellisFont = barPlot.Trellis.HeaderFont;
            Font newTrellisFont = new Font(trellisFont.FontFamily, 9, FontStyle.Bold);
            barPlot.Trellis.HeaderFont = newTrellisFont;
            barPlot.XAxis.TitleFont = newTrellisFont;

            barPlot.XAxis.ShowAxisSelector = false;
            barPlot.Legend.Visible = false;
            return barPlot.Visual;
        }

        [ActionDefinition("RefreshBarChartViz")]
        public void RefreshBarChartViz(Visual visual)
        {
            DataTable table = GetTableFromParameter("inputTable");
            if (table != null)
            {
                visual.As<BarChart>().Data.DataTableReference = table;
                visual.As<BarChart>().XAxis.Expression = "<" + GetColumnExpression(Scope["inputTable"].ToString(), Scope["samples"].ToString()) + ">";
                visual.As<BarChart>().YAxis.Expression = "Avg(" + GetColumnExpression(Scope["inputTable"].ToString(), Scope["values"].ToString()) + ")";
                //visual.As<BarChart>().ColorAxis.Expression = "<" + GetColumnExpression(Scope["inputTable"].ToString(), Scope["condition"].ToString()) + ">";
                visual.As<BarChart>().Data.WhereClauseExpression = GetColumnExpression(Scope["inputTable"].ToString(), Scope["features"].ToString()) + " = \"" + Scope["featureSelection"].ToString() + "\"";

            }
        }

        [VisualizationName("imageryScatterPlotViz")]
        public Visual CreateScatterPlotVisualization()
        {
            var scatterPlot = Page.Visuals.AddNew<ScatterPlot>();
            scatterPlot.AutoConfigure();

            scatterPlot.Title = "Distribution per Patient and Region";

            scatterPlot.Data.DataTableReference = GetTableFromParameter("inputTable");
            scatterPlot.XAxis.Expression = "<" + GetColumnExpression(Scope["inputTable"].ToString(), Scope["region"].ToString()) + ">";
            scatterPlot.YAxis.Expression = "Avg(" + GetColumnExpression(Scope["inputTable"].ToString(), Scope["values"].ToString()) + ") as [Value]";
            scatterPlot.Data.WhereClauseExpression = GetColumnExpression(Scope["inputTable"].ToString(), Scope["features"].ToString()) + " = \"" + Scope["featureSelection"].ToString() + "\" and" + GetColumnExpression(Scope["inputTable"].ToString(), Scope["values"].ToString()) + " is not null";
            scatterPlot.MarkerByAxis.Expression = "<" + GetColumnExpression(Scope["inputTable"].ToString(), Scope["samples"].ToString()) + ">";
            scatterPlot.Trellis.PageAxis.Expression = "<" + GetColumnExpression(Scope["inputTable"].ToString(), Scope["marker"].ToString()) + ">";
            scatterPlot.ColorAxis.Expression = "<" + GetColumnExpression(Scope["inputTable"].ToString(), Scope["region"].ToString()) + ">";
            scatterPlot.LineConnection.ConnectionAxis.Expression = "<" + GetColumnExpression(Scope["inputTable"].ToString(), Scope["samples"].ToString()) + ">";
            scatterPlot.YAxis.IndividualScaling = true;
            string hexValue = "#808080"; // Grey color
            Color colour = System.Drawing.ColorTranslator.FromHtml(hexValue);
            scatterPlot.LineConnection.Color = colour;
            scatterPlot.LineConnection.Width = 1;
            
            //Font of trellis header
            Font trellisFont = scatterPlot.Trellis.HeaderFont;
            Font newTrellisFont = new Font(trellisFont.FontFamily, 9, FontStyle.Bold);
            scatterPlot.Trellis.HeaderFont = newTrellisFont;
            scatterPlot.XAxis.TitleFont = newTrellisFont;
            //Legend configuration
            foreach (LegendItem i in scatterPlot.Legend.Items)
            {
                if (i.Title == "Color")
                {
                    i.Visible = true;
                    i.ShowTitle = false;
                }
                else
                {
                    i.Visible = false;
                }

            }
            scatterPlot.Legend.Width = 100;
            scatterPlot.YAxis.ManualZoom = true;
            return scatterPlot.Visual;
        }

        [ActionDefinition("RefreshScatterPlotViz")]
        public void RefreshScatterPlotViz(Visual visual)
        {
            DataTable table = GetTableFromParameter("inputTable");
            if (table != null)
            {
                visual.As<ScatterPlot>().Data.DataTableReference = table;
                visual.As<ScatterPlot>().XAxis.Expression = "<" + GetColumnExpression(Scope["inputTable"].ToString(), Scope["region"].ToString()) + ">";
                visual.As<ScatterPlot>().YAxis.Expression = "Avg(" + GetColumnExpression(Scope["inputTable"].ToString(), Scope["values"].ToString()) + ")";                
                visual.As<ScatterPlot>().Data.WhereClauseExpression = GetColumnExpression(Scope["inputTable"].ToString(), Scope["features"].ToString()) + " = \"" + Scope["featureSelection"].ToString() + "\" and" + GetColumnExpression(Scope["inputTable"].ToString(), Scope["values"].ToString()) + " is not null";
                visual.As<ScatterPlot>().MarkerByAxis.Expression = "<" + GetColumnExpression(Scope["inputTable"].ToString(), Scope["samples"].ToString()) + ">";
                visual.As<ScatterPlot>().Trellis.PageAxis.Expression = "<" + GetColumnExpression(Scope["inputTable"].ToString(), Scope["marker"].ToString()) + ">";
                visual.As<ScatterPlot>().ColorAxis.Expression = "<" + GetColumnExpression(Scope["inputTable"].ToString(), Scope["region"].ToString()) + ">";
                visual.As<ScatterPlot>().LineConnection.ConnectionAxis.Expression = "<" + GetColumnExpression(Scope["inputTable"].ToString(), Scope["samples"].ToString()) + ">";
            }
        }

        [VisualizationName("imageryLineChartViz")]
        public Visual CreateLineChartVisualization()
        {
            var linechart = Page.Visuals.AddNew<LineChart>();
            linechart.AutoConfigure();

            linechart.Title = "Aggregated value per region and marker";
            linechart.Visual.ShowTitle = false;

            linechart.Data.DataTableReference = GetTableFromParameter("inputTable");
            linechart.XAxis.Expression = "<" + GetColumnExpression(Scope["inputTable"].ToString(), Scope["region"].ToString()) + ">";
            linechart.YAxis.Expression = "Avg(" + GetColumnExpression(Scope["inputTable"].ToString(), Scope["values"].ToString()) + ") as [Value]";
            linechart.Data.WhereClauseExpression = GetColumnExpression(Scope["inputTable"].ToString(), Scope["features"].ToString()) + " = \"" + Scope["featureSelection"].ToString() + "\" and" + GetColumnExpression(Scope["inputTable"].ToString(), Scope["values"].ToString()) + " is not null";
            linechart.Trellis.PageAxis.Expression = "<Substitute(Substitute("+ GetColumnExpression(Scope["inputTable"].ToString(), Scope["marker"].ToString()) + ", \"CK+\", \"CK\"),\"CK-\",\"CK\")>";
            linechart.YAxis.IndividualScaling = true;
            linechart.YAxis.IndividualScalingMode = IndividualScalingMode.Trellis;
            string hexValue = "#808080"; // Grey color
            Color colour = System.Drawing.ColorTranslator.FromHtml(hexValue);
            linechart.ColorAxis.DefaultColor = Color.Blue;

            //Font of trellis header
            Font trellisFont = linechart.Trellis.HeaderFont;
            Font newTrellisFont = new Font(trellisFont.FontFamily, 9, FontStyle.Bold);
            linechart.Trellis.HeaderFont = newTrellisFont;
            linechart.XAxis.TitleFont = newTrellisFont;
            //Legend configuration
            linechart.Legend.Visible = false;
            //Error bars configuration
            linechart.YAxis.ErrorBars.Enabled = true;
            linechart.YAxis.ErrorBars.UpperExpression = "StdErr("+ GetColumnExpression(Scope["inputTable"].ToString(), Scope["values"].ToString())+ ")";
            linechart.YAxis.ErrorBars.LowerExpression = "StdErr(" + GetColumnExpression(Scope["inputTable"].ToString(), Scope["values"].ToString()) + ")";
            linechart.YAxis.ErrorBars.FixedColor = Color.Black;
            linechart.LabelVisibility = LabelVisibility.All;
            linechart.ShowMarkerLabels = true;
            linechart.ShowMarkers = true;
            linechart.LineWidth = 1;
            return linechart.Visual;
        }

        [VisualizationName("imageryLineChartCKViz")]
        public Visual CreateLineChartCKVisualization()
        {
            var linechart = Page.Visuals.AddNew<LineChart>();
            linechart.AutoConfigure();

            linechart.Title = "Aggregated value per region and marker";
            linechart.Visual.ShowTitle = false;
            linechart.Data.DataTableReference = GetTableFromParameter("inputTable");
            linechart.XAxis.Expression = "<If(Find(\"CK\",right(" + GetColumnExpression(Scope["inputTable"].ToString(), Scope["marker"].ToString()) + ",3))>0,right(" + GetColumnExpression(Scope["inputTable"].ToString(), Scope["marker"].ToString()) + ",3)) as [CK value]>";
            linechart.YAxis.Expression = "Avg(" + GetColumnExpression(Scope["inputTable"].ToString(), Scope["values"].ToString()) + ") as [Value]";
            linechart.Data.WhereClauseExpression = GetColumnExpression(Scope["inputTable"].ToString(), Scope["features"].ToString()) + " = \"" + Scope["featureSelection"].ToString() + "\" and" + GetColumnExpression(Scope["inputTable"].ToString(), Scope["values"].ToString()) + " is not null";
            linechart.Trellis.PageAxis.Expression = "<Substitute(Substitute(" + GetColumnExpression(Scope["inputTable"].ToString(), Scope["marker"].ToString()) + ", \"CK+\", \"CK\"),\"CK-\",\"CK\")>";
            linechart.YAxis.IndividualScaling = true;
            linechart.YAxis.IndividualScalingMode = IndividualScalingMode.Trellis;
            string hexValue = "#808080"; // Grey color
            Color colour = System.Drawing.ColorTranslator.FromHtml(hexValue);
            linechart.ColorAxis.DefaultColor = Color.Blue;

            //Font of trellis header
            Font trellisFont = linechart.Trellis.HeaderFont;
            Font newTrellisFont = new Font(trellisFont.FontFamily, 9, FontStyle.Bold);
            linechart.Trellis.HeaderFont = newTrellisFont;
            linechart.XAxis.TitleFont = newTrellisFont;
            //Legend configuration
            linechart.Legend.Visible = false;
            //Error bars configuration
            linechart.YAxis.ErrorBars.Enabled = true;
            linechart.YAxis.ErrorBars.UpperExpression = "StdErr(" + GetColumnExpression(Scope["inputTable"].ToString(), Scope["values"].ToString()) + ")";
            linechart.YAxis.ErrorBars.LowerExpression = "StdErr(" + GetColumnExpression(Scope["inputTable"].ToString(), Scope["values"].ToString()) + ")";
            linechart.YAxis.ErrorBars.FixedColor = Color.Black;
            linechart.LabelVisibility = LabelVisibility.All;
            linechart.ShowMarkerLabels = true;
            //Configure marking
            linechart.Data.LimitingMarkingsEmptyBehavior = LimitingMarkingsEmptyBehavior.ShowEmpty;
            linechart.Data.Filterings.Add(App.Document.Data.Markings["Marking"]);
            linechart.Data.LimitingMarkingsEmptyMessage = "Mark points in the visualizations to see CK analysis detail";
            linechart.Data.LimitingMarkingsEmptyBehavior = LimitingMarkingsEmptyBehavior.ShowMessage;
            linechart.Data.LimitingMarkingsEmptyMessageFont = newTrellisFont;
            linechart.ShowMarkers = true;
            linechart.LineWidth = 1;
            return linechart.Visual;
        }

        [VisualizationName("imageryBoxPlotViz")]
        public Visual CreateBoxPlotVisualization()
        {
            var boxPlot = Page.Visuals.AddNew<BoxPlot>();
            boxPlot.AutoConfigure();

            boxPlot.Title = "Distribution per Marker and Analysis region";

            boxPlot.Data.MarkingReference = App.Document.ActiveMarkingSelectionReference;
            //boxPlot.Data.LimitingMarkingsEmptyBehavior = LimitingMarkingsEmptyBehavior.ShowEmpty;
            //boxPlot.Data.LimitingMarkingsEmptyMessage = "No data to show - Img App";
            //boxPlot.Data.MarkingCombinationMethod = DataSelectionCombinationMethod.Union;
            boxPlot.Data.DataTableReference = GetTableFromParameter("inputTable");
            boxPlot.XAxis.Expression = "<" + GetColumnExpression(Scope["inputTable"].ToString(), Scope["region"].ToString()) + ">";
            boxPlot.YAxis.Expression = GetColumnExpression(Scope["inputTable"].ToString(), Scope["values"].ToString());
            boxPlot.ColorAxis.Expression = "<" + GetColumnExpression(Scope["inputTable"].ToString(), Scope["region"].ToString()) + ">";
            boxPlot.Data.WhereClauseExpression = GetColumnExpression(Scope["inputTable"].ToString(), Scope["features"].ToString()) + " = \"" + Scope["featureSelection"].ToString() + "\" and"+ GetColumnExpression(Scope["inputTable"].ToString(), Scope["values"].ToString())+" is not null";
            boxPlot.Trellis.PageAxis.Expression = "<" + GetColumnExpression(Scope["inputTable"].ToString(), Scope["marker"].ToString()) + ">";
            //Set reference points
            string hexValue = "#FF0000"; // Red color
            Color colour = System.Drawing.ColorTranslator.FromHtml(hexValue);
            foreach (BoxPlotReferencePoint point in boxPlot.ReferencePoints)
            {
                String name = point.MethodName;
                if (name == "Median")
                {
                    point.Visible = true;
                    point.ShowAsLine = true;
                    point.Color = colour;
                    point.LineStyle = LineStyle.Single;
                }
                else if (name == "Avg")
                {
                    point.Visible = true;
                    point.ShowAsLine = true;
                    point.Color = colour;
                    point.LineStyle = LineStyle.Dot;
                }
                else
                {
                    point.Visible = false;
                }
            }
            boxPlot.YAxis.ManualZoom = true;
            boxPlot.Table.Measures.Clear();
            boxPlot.Table.Measures.Add("Count");
            boxPlot.Table.Measures.Add("Median");
            boxPlot.Table.Measures.Add("Outliers");
            //Font of trellis header
            Font trellisFont = boxPlot.Trellis.HeaderFont;
            Font newTrellisFont = new Font(trellisFont.FontFamily, 9, FontStyle.Bold);
            boxPlot.Trellis.HeaderFont = newTrellisFont;
            //Legend configuration
            foreach ( LegendItem i in boxPlot.Legend.Items)
            {
                if (i.Title == "Reference points")
                {
                    i.Visible = true;
                    i.ShowTitle = false;

                }
                else
                {
                    i.Visible = false;
                }

            }
            boxPlot.Legend.Width = 100;
            return boxPlot.Visual;
        }

        [ActionDefinition("RefreshBoxPlotViz")]
        public void RefreshBoxPlotViz(Visual visual)
        {
            DataTable table = GetTableFromParameter("inputTable");
            if (table != null)
            {
                visual.As<BoxPlot>().Data.DataTableReference = table;
                visual.As<BoxPlot>().XAxis.Expression = "<" + GetColumnExpression(Scope["inputTable"].ToString(), Scope["region"].ToString()) + ">";
                visual.As<BoxPlot>().YAxis.Expression = GetColumnExpression(Scope["inputTable"].ToString(), Scope["values"].ToString());
                visual.As<BoxPlot>().ColorAxis.Expression = "<" + GetColumnExpression(Scope["inputTable"].ToString(), Scope["region"].ToString()) + ">";
                visual.As<BoxPlot>().Trellis.PageAxis.Expression = "<" + GetColumnExpression(Scope["inputTable"].ToString(), Scope["marker"].ToString()) + ">";
                visual.As<BoxPlot>().Data.WhereClauseExpression = GetColumnExpression(Scope["inputTable"].ToString(), Scope["features"].ToString()) + " = \"" + Scope["featureSelection"].ToString() + "\" and" + GetColumnExpression(Scope["inputTable"].ToString(), Scope["values"].ToString()) + " is not null";
            }
        }

        [VisualizationName("imageryTableViz")]
        public Visual CreateTableVisualization()
        {
            var tablePlot = Page.Visuals.AddNew<TablePlot>();
            tablePlot.Title = "Features per Sample";
            tablePlot.Data.DataTableReference = GetTableFromParameter("statResult");
            tablePlot.AutoConfigure();
            return tablePlot.Visual;
        }

        [VisualizationName("RefreshTableViz")]
        public void RefreshTableViz(Visual visual)
        {
            DataTable table = GetTableFromParameter("statResult");
            string testPerformed = "";
            if (table != null)
            {
                visual.As<TablePlot>().Data.DataTableReference = table;
                TablePlot tablePlot = visual.As<TablePlot>();
                DataColumnCollection dfColumns = visual.As<TablePlot>().Data.DataTableReference.Columns;
                List<DataColumn> listOfCols = new List<DataColumn>();
                DataValueCursor dv = DataValueCursor.CreateFormatted(table.Columns["test"]);
                foreach(DataRow row in table.GetRows(dv)){
                    testPerformed = dv.CurrentDataValue.ValidValue.ToString();
                    break;
                }

                for (int i=0;i< dfColumns.Count(); i++)
                {
                    if (!(dfColumns[i].Name == "W" && testPerformed == "t test") && !(dfColumns[i].Name == "t" && testPerformed == "Mann-Withney test") && !(dfColumns[i].Name == "degree_of_freedom" && testPerformed == "Mann-Withney test"))
                    {
                        listOfCols.Add(dfColumns[i]);
                    }
                }
                visual.As<TablePlot>().TableColumns.Clear();
                visual.As<TablePlot>().TableColumns.AddRange(listOfCols);
            }  
        }
    }
}

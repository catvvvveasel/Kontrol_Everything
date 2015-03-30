using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using VVVV.PluginInterfaces.V2;
using VVVV.Core.Logging;
using VVVV.Nodes.Table;

//#region DumpNodes
//public class DumpNode<T> : TablePluginEvaluate
//{
#region fields & pins
#pragma warning disable 169, 649
//[Output("Output")]
//ISpread<T> FOutput;
//[Output("Cell Count")]
//ISpread<int> FCellCount;
//[Output("Row Count")]
//ISpread<int> FRowCount;
#pragma warning restore
#endregion
//protected override void EvaluateTables(Spread<Table> tables, bool isChanged)
//{
//if (isChanged)
//{
//FOutput.SliceCount = 0;
//FCellCount.SliceCount = 0;
//FRowCount.SliceCount = 0;
//if (tables[0] != null)
//{
//for (int i=0; i<tables.SliceCount; i++)
//{
//int rows = tables[i].Rows.Count;
//for (int r=0; r<rows; r++)
//{
//List<T> result = tables[i].GetRow<T>(r);
//FOutput.AddRange(result);
//FCellCount.Add(result.Count);
//}
//FRowCount.Add(rows);
//}
//}
//}
//}
//}
//[PluginInfo(Name = "Dump", Category = TableDefaults.CATEGORY, Version = "Value", Help = "Gets values of all tables", Tags = TableDefaults.TAGS, Author = "elliotwoods, "+TableDefaults.AUTHOR)]
//public class DumpValueNode : DumpNode<double> {}
//[PluginInfo(Name = "Dump", Category = TableDefaults.CATEGORY, Version = "String", Help = "Gets strings of all tables", Tags = TableDefaults.TAGS, Author = TableDefaults.AUTHOR)]
//public class DumpStringNode : DumpNode<string> {}
//#endregion DumpNodes
#region GetTableNodes
public class GetTableNode<T> : TablePluginEvaluate
{
#region fields & pins
#pragma warning disable 169, 649
[Input("Index")]
IDiffSpread<int> FIndex;
[Output("Output")]
ISpread<T> FOutput;
[Output("Cell Count")]
ISpread<int> FCellCount;
[Output("Row Count")]
ISpread<int> FRowCount;
#pragma warning restore
#endregion
protected override void EvaluateTables(Spread<Table> tables, bool isChanged)
{
if (isChanged || FIndex.IsChanged)
{
FOutput.SliceCount = 0;
FCellCount.SliceCount = 0;
FRowCount.SliceCount = 0;
if (tables[0] != null)
{
for (int i=0; i<FIndex.SliceCount; i++)
{
int rows = tables[FIndex[i]].Rows.Count;
for (int r=0; r<rows; r++)
{
List<T> result = tables[FIndex[i]].GetRow<T>(r);
FOutput.AddRange(result);
FCellCount.Add(result.Count);
}
FRowCount.Add(rows);
}
}
}
}
}
[PluginInfo(Name = "GetTableByName", Category = TableDefaults.CATEGORY, Version = "Value", Help = "Gets values of an entire table", Tags = TableDefaults.TAGS, Author = TableDefaults.AUTHOR)]
public class GetTableValueNode : GetTableNode<double> {}
[PluginInfo(Name = "GetTableByName", Category = TableDefaults.CATEGORY, Version = "String", Help = "Gets strings of an entire table", Tags = TableDefaults.TAGS, Author = TableDefaults.AUTHOR)]
public class GetTableStringNode : GetTableNode<string> {}
#endregion GetTableNodes
#region GetRowNodes
//public class GetRowNode<T> : TablePluginEvaluate
//{
//#region fields & pins
//#pragma warning disable 169, 649
//[Input("Index")]
//IDiffSpread<int> FIndex;
//[Output("Output")]
//ISpread<ISpread<T>> FOutput;
//#pragma warning restore
//#endregion
//protected override void EvaluateTables(Spread<Table> tables, bool isChanged)
//{
//if (isChanged || FIndex.IsChanged)
//{
//if (tables[0] == null)
//FOutput.SliceCount = 0;
//else
//{
//int spreadMax = tables.SliceCount.CombineWith(FIndex);
//FOutput.SliceCount = spreadMax;
//for (int i=0; i<spreadMax; i++)
//FOutput[i].AssignFrom(tables[i].GetRow<T>(FIndex[i]));
//}
//}
//}
//}
//[PluginInfo(Name = "GetRow", Category = TableDefaults.CATEGORY, Version = "Value", Help = "Gets values of a table row", Tags = TableDefaults.TAGS, Author = TableDefaults.AUTHOR)]
//public class GetRowValueNode : GetRowNode<double> {}
//[PluginInfo(Name = "GetRow", Category = TableDefaults.CATEGORY, Version = "String", Help = "Gets strings of a table row", Tags = TableDefaults.TAGS, Author = TableDefaults.AUTHOR)]
//public class GetRowStringNode : GetRowNode<string> {}
//#endregion GetRowNodes
//#region GetColumnNodes
//public class GetColumnNode<T> : TablePluginEvaluate
//{
//#region fields & pins
//#pragma warning disable 169, 649
//[Input("Index")]
//IDiffSpread<int> FIndex;
//[Output("Output")]
//ISpread<ISpread<T>> FOutput;
//#pragma warning restore
//#endregion
//protected override void EvaluateTables(Spread<Table> tables, bool isChanged)
//{
//if (isChanged || FIndex.IsChanged)
//{
//if (tables[0] == null)
//FOutput.SliceCount = 0;
//else
//{
//int spreadMax = tables.SliceCount.CombineWith(FIndex);
//FOutput.SliceCount = spreadMax;
//for (int i=0; i<spreadMax; i++)
//FOutput[i].AssignFrom(tables[i].GetColumn<T>(FIndex[i]));
//}
//}
//}
//}
//[PluginInfo(Name = "GetColumn", Category = TableDefaults.CATEGORY, Version = "Value", Help = "Gets values of a table column", Tags = TableDefaults.TAGS, Author = TableDefaults.AUTHOR)]
//public class GetColumnValueNode : GetColumnNode<double> {}
//[PluginInfo(Name = "GetColumn", Category = TableDefaults.CATEGORY, Version = "String", Help = "Gets strings of a table column", Tags = TableDefaults.TAGS, Author = TableDefaults.AUTHOR)]
//public class GetColumnStringNode : GetColumnNode<string> {}
//#endregion GetColumnNodes
//#region GetCellNodes
//public class GetCellNode<T> : TablePluginEvaluate
//{
//#region fields & pins
//#pragma warning disable 169, 649
//[Input("Row Index", BinVisibility = PinVisibility.OnlyInspector, BinSize = 1)]
//IDiffSpread<ISpread<int>> FRowId;
//[Input("Column Index", BinVisibility = PinVisibility.OnlyInspector, BinSize = 1)]
//IDiffSpread<ISpread<int>> FColId;
//[Output("Output", BinVisibility = PinVisibility.OnlyInspector)]
//ISpread<ISpread<T>> FOutput;
//#pragma warning restore
//#endregion
//protected override void EvaluateTables(Spread<Table> tables, bool isChanged)
//{
//if (isChanged || FRowId.IsChanged || FColId.IsChanged)
//{
//if (tables[0] == null)
//FOutput.SliceCount = 0;
//else
//{
//int binCount = tables.SliceCount.CombineWith(FRowId).CombineWith(FColId);
//FOutput.SliceCount = binCount;
//for (int i=0; i<binCount; i++)
//{
//int sliceCount = FRowId[i].SliceCount.CombineWith(FColId[i]);
//FOutput[i].SliceCount = sliceCount;
//for (int j=0; j<sliceCount; j++)
//FOutput[i][j]=tables[i].Get<T>(FRowId[i][j],FColId[i][j]);
//}
//}
//}
//}
//}
//[PluginInfo(Name = "GetCell", Category = TableDefaults.CATEGORY, Version = "Value", Help = "Gets the value of a table cell", Tags = TableDefaults.TAGS, Author = TableDefaults.AUTHOR)]
//public class GetCellValueNode : GetCellNode<double> {}
//[PluginInfo(Name = "GetCell", Category = TableDefaults.CATEGORY, Version = "String", Help = "Gets the string of a table cell", Tags = TableDefaults.TAGS, Author = TableDefaults.AUTHOR)]
//public class GetCellStringNode : GetCellNode<string> {}
//#endregion GetCellNodes

#endregion
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using VVVV.PluginInterfaces.V2;
using VVVV.Core.Logging;
using VVVV.Nodes.Table;

namespace VVVV.Nodes.Table
{

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


	}


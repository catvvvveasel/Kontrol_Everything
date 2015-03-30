#region usings
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using VVVV.PluginInterfaces.V2;
using VVVV.Core.Logging;


	

#endregion usings

namespace VVVV.Nodes
{
	#region PluginInfo
	[PluginInfo(Name = "GetByName", Category = "String", Help = "Basic template with one string in/out", Tags = "")]
	#endregion PluginInfo
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
}
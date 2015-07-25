﻿using System;
using System.Collections.Generic;
using System.Data;
using BrainSharper.Abstract.Algorithms.DecisionTrees.DataStructures;
using BrainSharper.Abstract.Algorithms.DecisionTrees.DataStructures.BinaryTrees;
using BrainSharper.Abstract.Algorithms.DecisionTrees.Processors;
using BrainSharper.Abstract.Data;
using BrainSharper.Implementations.Algorithms.DecisionTrees.DataStructures;
using BrainSharper.Implementations.Algorithms.DecisionTrees.DataStructures.BinaryDecisionTrees;

namespace BrainSharper.Implementations.Algorithms.DecisionTrees.Processors
{
    public class BinaryDiscreteDataSplitter<T> : IBinaryDataSplitter<T>
    {
        public IList<ISplittedData<bool>> SplitData(IDataFrame dataToSplit, IBinarySplittingParams<T> splttingParams)
        {
            var splittingFeatureName = splttingParams.SplitOnFeature;
            var splittingFeatureValue = splttingParams.SplitOnValue;

            var rowFilter = BuildSplittingFunction(splittingFeatureName, splittingFeatureValue);

            var filteringResult = dataToSplit.GetRowsIndicesWhere(rowFilter);
            var rowsMeetingCriteria = filteringResult.IndicesOfRowsMeetingCriteria;
            var rowsNotMeetingCriteria = filteringResult.IndicesOfRowsNotMeetingCriteria;

            var splitResults = new List<ISplittedData<bool>>();
            var totalRowsCount = (double)dataToSplit.RowCount;

            var positiveDataFrame = dataToSplit.GetSubsetByRows(rowsMeetingCriteria);
            splitResults.Add(new SplittedData<bool>(GetSubsetLink(positiveDataFrame, totalRowsCount, true), positiveDataFrame));

            var negativeDataFrame = dataToSplit.GetSubsetByRows(rowsNotMeetingCriteria);
            splitResults.Add(new SplittedData<bool>(GetSubsetLink(negativeDataFrame, totalRowsCount, false), negativeDataFrame));

            return splitResults;
        }

        public IList<ISplittedData<bool>> SplitData(
            IDataFrame dataToSplit,
            ISplittingParams splttingParams)
        {
            if (!(splttingParams is IBinarySplittingParams<T>))
            {
                throw new ArgumentException("Invalid splitting params passed to binary splitter");
            }
            return SplitData(dataToSplit, (IBinarySplittingParams<T>) splttingParams);
        }

        protected virtual Predicate<DataRow> BuildSplittingFunction(string splittingFeatureName, T splittingFeatureValue)
        {
            Predicate<DataRow> rowFilter =
                row => Convert.ChangeType(row[splittingFeatureName], typeof(T))
                    .Equals(splittingFeatureValue);
            return rowFilter;
        }

        private static BinaryDecisionTreeLink GetSubsetLink(IDataFrame subset, double totalRowsCount, bool testResult)
        {
            return new BinaryDecisionTreeLink(
                subset.Any ? subset.RowCount/totalRowsCount : 0,
                subset.RowCount,
                testResult);
        }
    }
}
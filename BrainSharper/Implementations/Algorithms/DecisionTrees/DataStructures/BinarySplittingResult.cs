﻿using System.Collections.Generic;
using BrainSharper.Abstract.Algorithms.DecisionTrees.DataStructures;

namespace BrainSharper.Implementations.Algorithms.DecisionTrees.DataStructures
{
    public class BinarySplittingResult : SplittingResult<bool>, IBinarySplittingResult
    {
        public BinarySplittingResult(
            bool isSplitNumeric, 
            string splittingFeatureName, 
            IList<ISplittedData<bool>> splittedDataSets, 
            object splittingValue) 
            : base(isSplitNumeric, splittingFeatureName, splittedDataSets)
        {
            SplittingValue = splittingValue;
        }

        public object SplittingValue { get; }
    }
}
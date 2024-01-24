using System;
using System.Collections.Generic;

namespace Utilities
{
    public class MinMax<T>
    {
        private readonly Func<T, double> _evaluate;
        private readonly Func<T, List<T>> _getChildrenF;
        
        public MinMax(Func<T, double> evaluate, Func<T, List<T>> getChildren)
        {
            _evaluate = evaluate;
            _getChildrenF = getChildren;
        }
        
        public T FindBestMove(T targetPosition, int depth, bool isMaximizingPlayer)
        {
            T bestMove = default(T);
            var root = new MinMaxNode<T>(targetPosition, _getChildrenF);
            
            if (isMaximizingPlayer)
            {
                double bestValue = double.NegativeInfinity;
                foreach (var child in root.Children)
                {
                    double value = MinMaxValue(child, depth - 1, false, double.NegativeInfinity,
                        double.PositiveInfinity);
                    if (value > bestValue)
                    {
                        bestValue = value;
                        bestMove = child.Value;
                    }
                }
            }
            else
            {
                double bestValue = double.PositiveInfinity;
                foreach (var child in root.Children)
                {
                    double value = MinMaxValue(child, depth - 1, true, double.NegativeInfinity,
                        double.PositiveInfinity);
                    if (value < bestValue)
                    {
                        bestValue = value;
                        bestMove = child.Value;
                    }
                }
            }

            return bestMove;
        }

        private double MinMaxValue(MinMaxNode<T> node, int depth, bool isMaximizingPlayer, double alpha, double beta)
        {
            if (depth == 0 || node.Children.Count == 0)
            {
                return _evaluate(node.Value);
            }

            if (isMaximizingPlayer)
            {
                double bestValue = double.NegativeInfinity;
                foreach (var child in node.Children)
                {
                    double value = MinMaxValue(child, depth - 1, false, alpha, beta);
                    bestValue = Math.Max(bestValue, value);
                    alpha = Math.Max(alpha, bestValue);
                    if (beta <= alpha)
                        break;
                }

                return bestValue;
            }
            else
            {
                double bestValue = double.PositiveInfinity;
                foreach (var child in node.Children)
                {
                    double value = MinMaxValue(child, depth - 1, true, alpha, beta);
                    bestValue = Math.Min(bestValue, value);
                    beta = Math.Min(beta, bestValue);
                    if (beta <= alpha)
                        break;
                }

                return bestValue;
            }
        }
        
        private class MinMaxNode<T>
        {
            public T Value { get; set; }

            public List<MinMaxNode<T>> Children
            {
                get
                {
                    if (_children == null)
                    {
                        _children = new List<MinMaxNode<T>>();
                        ExpendChildren();
                    }

                    return _children;
                }
            }

            private List<MinMaxNode<T>> _children;
            private readonly Func<T,List<T>> _getChildren;

            public MinMaxNode(T value, Func<T, List<T>> getChildren)
            {
                _getChildren = getChildren;
                Value = value;
            }
            
            private void ExpendChildren()
            {
                foreach (var child in _getChildren(Value))
                {
                    Children.Add(new MinMaxNode<T>(child, _getChildren));
                }
            }
        }
    }
}
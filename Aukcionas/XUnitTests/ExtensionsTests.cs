using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using System;
using Aukcionas.Utils;

namespace XUnitTests
{
    public class ExtensionsTests
    {
        [Fact]
        public void CombinePredicatesWithAnd_EmptyList_ReturnsTruePredicate()
        {
            var predicates = new List<Expression<Func<int, bool>>>();
            var combinedPredicate = Extensions.CombinePredicatesWithAnd(predicates);
            Assert.NotNull(combinedPredicate);
            var compiledPredicate = combinedPredicate.Compile();
            Assert.True(compiledPredicate(123));
        }

        [Fact]
        public void CombinePredicatesWithAnd_SinglePredicate_ReturnsSamePredicate()
        {
            var predicate = CreatePredicate<int>(x => x > 10);
            var predicates = new List<Expression<Func<int, bool>>> { predicate };
            var combinedPredicate = Extensions.CombinePredicatesWithAnd(predicates);
            Assert.NotNull(combinedPredicate);
            var compiledPredicate = combinedPredicate.Compile();
            Assert.True(compiledPredicate(15));
            Assert.False(compiledPredicate(5));
        }

        [Fact]
        public void CombinePredicatesWithAnd_MultiplePredicates_ReturnsCombinedPredicate()
        {
            var predicate1 = CreatePredicate<int>(x => x > 10);
            var predicate2 = CreatePredicate<int>(x => x % 2 == 0);
            var predicates = new List<Expression<Func<int, bool>>> { predicate1, predicate2 };
            var combinedPredicate = Extensions.CombinePredicatesWithAnd(predicates);
            Assert.NotNull(combinedPredicate);
            var compiledPredicate = combinedPredicate.Compile();
            Assert.True(compiledPredicate(12));
            Assert.False(compiledPredicate(15));
            Assert.False(compiledPredicate(8));
        }
        [Fact]
        public void CombinePredicatesWithAnd_AllPredicatesTrue_ReturnsTrue()
        {
            var predicates = new List<Expression<Func<int, bool>>>
            {
                CreatePredicate<int>(x => x > 0),
                CreatePredicate<int>(x => x < 100),
                CreatePredicate<int>(x => x % 2 == 0)
            };
            var combinedPredicate = Extensions.CombinePredicatesWithAnd(predicates);
            Assert.NotNull(combinedPredicate);
            var compiledPredicate = combinedPredicate.Compile();
            Assert.True(compiledPredicate(50));
        }

        [Fact]
        public void CombinePredicatesWithAnd_AnyPredicateFalse_ReturnsFalse()
        {
            var predicates = new List<Expression<Func<int, bool>>>
            {
                CreatePredicate<int>(x => x > 0),
                CreatePredicate<int>(x => x < 100),
                CreatePredicate<int>(x => x % 2 == 0)
            };
            var combinedPredicate = Extensions.CombinePredicatesWithAnd(predicates);
            Assert.NotNull(combinedPredicate);
            var compiledPredicate = combinedPredicate.Compile();
            Assert.False(compiledPredicate(101));
            Assert.False(compiledPredicate(3));
        }

        private Expression<Func<T, bool>> CreatePredicate<T>(Expression<Func<T, bool>> predicate)
        {
            return predicate;
        }
    }
}

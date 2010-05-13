﻿using NUnit.Framework;
using SpecUnit;
using TwitterLib;

namespace WittyUnitTests
{
    [TestFixture]
    public class TweetTest
    {
        [SetUp]
        public void Init()
        {
        }

        [Test]
        public void Truncate()
        {
            int count = 10;

            TweetCollection tweets = new TweetCollection();

            for (int i = 0; i < 20; i++)
            {
                Tweet tweet = new Tweet();
                tweet.Id = i;
                tweets.Insert(0, tweet);
            }

            tweets.TruncateAfter(count);

            Assert.AreEqual(count, tweets.Count);
            Assert.AreEqual(tweets[0].Id, 19);
        }
    }
    /*

 * NOTE: SpecUnit isn't working with with TestDriven.net nor ReSharper TestRunner right now,
 * so we'll just use the standard NUnit attributes until SpecUnit is fixed.
 */
    //    [Concern(typeof(TweetCollection))]
    [TestFixture]
    public class when_a_collection_of_tweets_is_truncated : ContextSpecification
    {
        private TweetCollection _tweets;
        private int _totalTweets, _keepCount;
        protected override void Context()
        {
            _tweets = new TweetCollection();
            _totalTweets = 20;
            _keepCount = 5;
            for (int i = 1; i <= _totalTweets; i++)
            {
                var tweet = new Tweet { Id = i };
                _tweets.Insert(0, tweet);
                _tweets.Insert(0, tweet);
            }
        }

        protected override void Because()
        {
            _tweets.TruncateAfter(_keepCount);
        }

        //        [Observation]
        [Test]
        public void then_should_remove_excess_tweets()
        {
            _tweets.Count.ShouldEqual(_keepCount);
        }

        //[Test]
        //public void then_should_remove_tweets_from_collection_tail()
        //{
        //    _tweets.First().Id.ShouldEqual(20);
        //    _tweets.Last().Id.ShouldEqual(16);
        //}
    }
}


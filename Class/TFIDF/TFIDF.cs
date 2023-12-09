using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Class.TFIDF
{
    public class TFIDF
    {
        private List<string> _documents;
        private Dictionary<string, double> _idfScores;

        public TFIDF(List<string> documents)
        {
            _documents = documents;
            _idfScores = CalculateIDFScores();
        }

        private Dictionary<string, double> CalculateIDFScores()
        {
            var idfScores = new Dictionary<string, double>();

            var totalDocuments = _documents.Count;
            var termCounts = new Dictionary<string, int>();

            foreach (var document in _documents)
            {
                var terms = document.Split(' ');

                foreach (var term in terms.Distinct())
                {
                    if (termCounts.ContainsKey(term))
                    {
                        termCounts[term]++;
                    }
                    else
                    {
                        termCounts[term] = 1;
                    }
                }
            }

            foreach (var termCount in termCounts)
            {
                idfScores[termCount.Key] = Math.Log(totalDocuments / (double)termCount.Value);
            }

            return idfScores;
        }

        public double[][] Transform()
        {
            var vectors = new List<double[]>();

            foreach (var document in _documents)
            {
                var terms = document.Split(' ');
                var vector = new double[_idfScores.Count];

                for (var i = 0; i < _idfScores.Count; i++)
                {
                    var term = _idfScores.ElementAt(i).Key;
                    var tf = terms.Count(t => t == term);
                    var idf = _idfScores[term];

                    vector[i] = tf * idf;
                }

                vectors.Add(vector);
            }

            return vectors.ToArray();
        }

        public static double CosineSimilarity(double[] vec1, double[] vec2)
        {
            var dotProduct = DotProduct(vec1, vec2);
            var magnitude1 = Magnitude(vec1);
            var magnitude2 = Magnitude(vec2);

            return dotProduct / (magnitude1 * magnitude2);
        }

        public static double DotProduct(double[] vec1, double[] vec2)
        {
            double dotProduct = 0;

            for (int i = 0; i < vec1.Length; i++)
            {
                dotProduct += vec1[i] * vec2[i];
            }

            return dotProduct;
        }

        public static double Magnitude(double[] vec)
        {
            double sumOfSquares = 0;

            for (int i = 0; i < vec.Length; i++)
            {
                sumOfSquares += Math.Pow(vec[i], 2);
            }

            return Math.Sqrt(sumOfSquares);
        }

        public static List<int> Search(string query, double[][] documentVectors, TFIDF tfidf)
        {
            var queryTerms = query.Split(' ');
            var queryVector = new double[tfidf._idfScores.Count];

            for (var i = 0; i < tfidf._idfScores.Count; i++)
            {
                var term = tfidf._idfScores.ElementAt(i).Key;
                var tf = queryTerms.Count(t => t == term);
                var idf = tfidf._idfScores[term];

                queryVector[i] = tf * idf;
            }

            var similarities = new List<double>();

            foreach (var documentVector in documentVectors)
            {
                var similarity = CosineSimilarity(queryVector, documentVector);
                similarities.Add(similarity);
            }

            var sortedIndices = similarities
                .Select((s, i) => new { Index = i , Similarity = s })
                .OrderByDescending(x => x.Similarity )
                .Where(x => x.Similarity > 0)
                .Select(x => x.Index + 1)
                .ToList();

            return sortedIndices;
        }


    }



}
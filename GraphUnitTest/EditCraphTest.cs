using Microsoft.VisualStudio.TestTools.UnitTesting;
using EditGraph;

namespace GraphUnitTest
{
    [TestClass]
    public class EditCraphTest
    {
        [TestMethod]
        public void TestAddVertex()
        {
            GraphWirth a = new GraphWirth();
            bool expected = true;
            bool actual = a.AddVertex(15);
            Assert.AreEqual(expected, actual, "Error adding vertex.");
        }

        [TestMethod]
        public void TestAddVertex_WithExistingID()
        {
            GraphWirth a = new GraphWirth();
            bool expected = false;
            a.AddVertex(15);
            bool actual = a.AddVertex(15);
            Assert.AreEqual(expected, actual, "Vertex with this key is already exists.");
        }

        [TestMethod]
        public void TestDeleteVertex()
        {
            GraphWirth a = new GraphWirth();
            bool expected = true;
            a.AddVertex(15, 20);
            a.AddVertex(20);
            bool actual = a.DeleteVertex(15);
            Assert.AreEqual(expected, actual, "Unable to delete vertex.");
        }

        [TestMethod]
        public void TestDeleteVertex_NotExistant()
        {
            GraphWirth a = new GraphWirth();
            bool expected = false;
            bool actual = a.DeleteVertex(15);
            Assert.AreEqual(expected, actual, "Non-existant vertex can not be deleated.");
        }

        [TestMethod]
        public void TestDeleteVertex_LastVertex()
        {
            GraphWirth a = new GraphWirth();
            bool expected = true;
            a.AddVertex(10);
            a.AddVertex(100);
            a.AddVertex(15, 1000);
            bool actual = a.DeleteVertex(15);
            Assert.AreEqual(expected, actual, "Error deleting last vertex.");
        }

        [TestMethod]
        public void TestDeleteVertex_LastOfTwo()
        {
            GraphWirth a = new GraphWirth();
            bool expected = true;
            a.AddVertex(15, 1000);
            bool actual = a.DeleteVertex(15);
            Assert.AreEqual(expected, actual, "Error deleting last of two vertexes.");
        }

        [TestMethod]
        public void TestDeleteVertex_FirstOfMany()
        {
            GraphWirth a = new GraphWirth();
            bool expected = true;
            a.AddVertex(10);
            a.AddVertex(100);
            a.AddVertex(1000);
            bool actual = a.DeleteVertex(1);
            Assert.AreEqual(expected, actual, "Error deleting first vertex.");
        }

        [TestMethod]
        public void TestAddDirectEdge()
        {
            GraphWirth a = new GraphWirth();
            bool expected = true;
            a.AddVertex(0);
            bool actual = a.AddDirectEdge(1, 2, 0);
            Assert.AreEqual(expected, actual, "Error adding direct edge.");
        }

        [TestMethod]
        public void TestAddUndirectEdge()
        {
            GraphWirth a = new GraphWirth();
            bool expected = true;
            a.AddVertex(0);
            bool actual = a.AddUndirectEdge(1, 2, 0);
            Assert.AreEqual(expected, actual, "Error adding undirect edge.");
        }

        [TestMethod]
        public void TestDeleteDirectEdge_FromTo()
        {
            GraphWirth a = new GraphWirth();
            bool expected = true;
            a.AddVertex(0);
            a.AddDirectEdge(1, 2, 0);
            bool actual = a.DeleteDirectEdge(1, 2);
            Assert.AreEqual(expected, actual, "Error deleting direct edge.");
        }

        [TestMethod]
        public void TestDeleteDirectEdge_ToFrom()
        {
            GraphWirth a = new GraphWirth();
            bool expected = false;
            a.AddVertex(0);
            a.AddDirectEdge(1, 2, 0);
            bool actual = a.DeleteDirectEdge(2, 1);
            Assert.AreEqual(expected, actual, "Incorrect order of vertexes while deleting direct edge.");
        }

        [TestMethod]
        public void TestDeleteUndirectEdge_FromTo()
        {
            GraphWirth a = new GraphWirth();
            bool expected = true;
            a.AddVertex(0);
            a.AddUndirectEdge(1, 2, 0);
            bool actual = a.DeleteUndirectEdge(1, 2);
            Assert.AreEqual(expected, actual, "Error deleting undirect edge.");
        }

        [TestMethod]
        public void TestDeleteUndirectEdge_ToFrom()
        {
            GraphWirth a = new GraphWirth();
            bool expected = true;
            a.AddVertex(0);
            a.AddDirectEdge(1, 2, 0);
            bool actual = a.DeleteDirectEdge(2, 1);
            Assert.AreEqual(expected, actual, "Error deleting undirect edge.");
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;
using ObjLoader.Loader.Data.Elements;
using ObjLoader.Loader.Loaders;

namespace DynModelLoader
{
    /// <summary>
    /// Main class for loading 3D files
    /// </summary>
    public static class ModelLoader
    {
        /// <summary>
        /// Loads geometry from an OBJ file into native Dynamo geometry.
        /// </summary>
        /// <param name="filePath">The filepath of the .OBJ file.</param>
        /// <returns>List of OBJ Groups as Dynamo PolySurfaces.</returns>
        public static List<PolySurface> LoadObj(string filePath)
        {
            var loaderFactory = new ObjLoaderFactory();
            var loader = loaderFactory.Create();

            var fs = new FileStream(filePath, FileMode.Open);
            var result = loader.Load(fs);
            
            fs.Close();

            List<PolySurface> polySurfaces = new List<PolySurface>();

            foreach(var group in result.Groups)
            {
                polySurfaces.Add(GetObjGroupMesh(group, result));
            }

            return polySurfaces;
        }

        private static PolySurface GetObjGroupMesh(Group group, LoadResult result)
        {
            var polygons = new List<Surface>();
            var points = new List<Point>();

            foreach (var face in group.Faces)
            {
                for (int i = 0; i < face.Count; i++)
                {
                    var index = face[i].VertexIndex - 1;
                    points.Add(Point.ByCoordinates(result.Vertices[index].X, result.Vertices[index].Y, result.Vertices[index].Z));
                }
            }

            for (int i = 0; i < points.Count; i += 3)
            {
                int j = i + 1;
                int k = i + 2;

                var point1 = points[i];
                var point2 = points[j];
                var point3 = points[k];

                var facePoints = new List<Point>() { point1, point2, point3 };
                try
                {
                    polygons.Add(Surface.ByPerimeterPoints(facePoints));
                    point1.Dispose();
                    point2.Dispose();
                    point3.Dispose();
                }
                catch
                {
                    point1.Dispose();
                    point2.Dispose();
                    point3.Dispose();
                    continue;
                }
            }

            return PolySurface.ByJoinedSurfaces(polygons);
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;

using Assimp;

namespace DynModelLoader
{
    /// <summary>
    /// Main class for loading 3D files
    /// </summary>
    public static class ModelLoader
    {
        /// <summary>
        /// Loads geometry from an OBJ file into native Dynamo geometry. Uses Assimp
        /// </summary>
        /// <param name="filePath">The filepath of the 3D mesh.</param>
        /// <returns>List of OBJ Groups as Dynamo PolySurfaces.</returns>
        public static List<PolySurface> LoadMesh(string filePath)
        {
            List<PolySurface> polySurfaces = new List<PolySurface>();

            using(var loader = new AssimpContext())
            {
                Scene scene;
                try
                {
                    scene = loader.ImportFile(filePath, PostProcessSteps.Triangulate);
                }
                catch(AssimpException ex)
                {
                    throw new Exception(ex.Message);
                }
                if(scene.HasMeshes)
                {
                    foreach(var mesh in scene.Meshes)
                    {
                        polySurfaces.Add(GetPolySurfaceFromAssimpMesh(mesh));
                    }
                }
            }
            return polySurfaces;
        }

        private static PolySurface GetPolySurfaceFromAssimpMesh (Assimp.Mesh mesh)
        {
            var polygons = new List<Surface>();
            var points = new List<Point>();

            foreach(var face in mesh.Faces)
            {
                foreach(var index in face.Indices)
                {
                    points.Add(Point.ByCoordinates(mesh.Vertices[index].X, mesh.Vertices[index].Y, mesh.Vertices[index].Z));
                }
            }

            for(int i = 0; i < points.Count; i += 3)
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
            var pSurface = PolySurface.ByJoinedSurfaces(polygons);
            pSurface.Rotate(Point.Origin(), Vector.XAxis(), 180.0);
            pSurface.Rotate(Point.Origin(), Vector.YAxis(), 90.0);
            return pSurface;
        }
    }
}

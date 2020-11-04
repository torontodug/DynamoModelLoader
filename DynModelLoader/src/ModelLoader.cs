using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.DesignScript.Geometry;

using Assimp;

namespace DynModelLoader
{
    /// <summary>
    /// Main class for loading 3D files
    /// </summary>
    public static class ModelLoader
    {
        /// <summary>
        /// Loads geometry from a 3D file into native Dynamo geometry.
        /// </summary>
        /// <param name="filePath">The filepath of the 3D mesh file.</param>
        /// <returns>List of mesh groups as Dynamo PolySurfaces.</returns>
        public static List<Autodesk.DesignScript.Geometry.Mesh> LoadMeshToDynamoMesh(string filePath)
        {
            List<Autodesk.DesignScript.Geometry.Mesh> dynMeshes = new List<Autodesk.DesignScript.Geometry.Mesh>();

            using (var loader = new AssimpContext())
            {
                Scene scene;
                try
                {
                    scene = loader.ImportFile(filePath, PostProcessSteps.Triangulate |
                                                        PostProcessSteps.FixInFacingNormals |
                                                        PostProcessSteps.SplitLargeMeshes |
                                                        PostProcessSteps.OptimizeMeshes |
                                                        PostProcessSteps.OptimizeGraph |
                                                        PostProcessSteps.JoinIdenticalVertices |
                                                        PostProcessSteps.FindDegenerates);
                }
                catch (AssimpException ex)
                {
                    throw new Exception(ex.Message);
                }
                if (scene.HasMeshes)
                {
                    foreach (var mesh in scene.Meshes)
                    {
                        dynMeshes.Add(GetDynamoMeshFromAssimpMesh(mesh));
                    }
                }
            }
            return dynMeshes;
        }

        private static Autodesk.DesignScript.Geometry.Mesh GetDynamoMeshFromAssimpMesh(Assimp.Mesh mesh)
        {
            var meshIndices = new List<IndexGroup>();
            // Go through indices of the mesh
            var assimpMeshIndices = mesh.GetIndices();
            for (int i = 0; i < assimpMeshIndices.Length; i+=3)
            {
                meshIndices.Add(IndexGroup.ByIndices(
                    (uint)assimpMeshIndices[i], 
                    (uint)assimpMeshIndices[i + 1], 
                    (uint)assimpMeshIndices[i + 2]));
            }

            var meshVertices = new List<Point>();
            // Go through the vertices of the mesh
            var assimpMeshVertices = mesh.Vertices;
            for (int i = 0; i < assimpMeshVertices.Count; i++)
            {
                meshVertices.Add(Point.ByCoordinates(
                    assimpMeshVertices[i].X, 
                    assimpMeshVertices[i].Y, 
                    assimpMeshVertices[i].Z));
            }

            return Autodesk.DesignScript.Geometry.Mesh.ByPointsFaceIndices(meshVertices, meshIndices);
        }
    }
}

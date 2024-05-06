﻿using Fishing_SharpDX.Graphics;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System;
using System.Runtime.InteropServices;
using Buffer11 = SharpDX.Direct3D11.Buffer;

namespace Fishing_SharpDX.Objects
{
    public class MeshObject : PositionalObject, IDisposable
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct VertexDataStruct
        {
            public Vector4 position;
            public Vector4 normal;
            public Vector4 color;
            public Vector2 texCoord0;
        }
        public bool IsVisible { get; set; }
        public bool IsMoveable { get; set; }
        public string Name { get; set; }
        public int Index { get; set; }

        private DirectX3DGraphics _directX3DGraphics;

        private Renderer _renderer;

        #region Vertices and Indexes
        /// <summary>Count of object vertices.</summary>
        private int _verticesCount;

        /// <summary>Array of vertex data.</summary>
        private VertexDataStruct[] _vertices;

        /// <summary>Vertex buffer DirectX object.</summary>
        private Buffer11 _vertexBufferObject;

        private VertexBufferBinding _vertexBufferBinding;

        /// <summary>Count of object vertex Indexes.</summary>
        private int _indexesCount;

        /// <summary>Array of object vertex indexes.</summary>
        private uint[] _indexes;

        private Buffer11 _indexBufferObject;
        #endregion

        private Material _material;
        public Material Material { get => _material; set => _material = value; }

        public MeshObject(string name, DirectX3DGraphics directX3DGraphics, Renderer renderer,
                        Vector4 initialPosition,
                        VertexDataStruct[] vertices, uint[] indexes, Material material) :
                        base(initialPosition)
        {
            IsMoveable = true;
            IsVisible = true;
            Name = name;
            _directX3DGraphics = directX3DGraphics;
            _renderer = renderer;
            if (null != vertices)
            {
                _vertices = vertices;
                _verticesCount = _vertices.Length;
            }
            if (null != indexes)
            {
                _indexes = indexes;
                _indexesCount = _indexes.Length;
            }
            else
            {
                _indexesCount = _verticesCount;
                _indexes = new uint[_indexesCount];
                for (int i = 0; i <= _indexesCount; ++i) _indexes[i] = (uint)i;
            }
            _material = material;

            _vertexBufferObject = Buffer11.Create(_directX3DGraphics.Device, BindFlags.VertexBuffer, _vertices, Utilities.SizeOf<VertexDataStruct>() * _verticesCount);
            _vertexBufferBinding = new VertexBufferBinding(_vertexBufferObject, Utilities.SizeOf<VertexDataStruct>(), 0);
            _indexBufferObject = Buffer11.Create(_directX3DGraphics.Device, BindFlags.IndexBuffer, _indexes, Utilities.SizeOf<int>() * _indexesCount);

        }

        public MeshObject(Vector4 initialPosition) : base(initialPosition) { IsMoveable = true; }

        public virtual void Render(Matrix viewMatrix, Matrix projectionMatrix)
        {
            if (!IsVisible) return;
            _renderer.UpdatePerObjectConstantBuffer(0, GetWorldMatrix(), viewMatrix, projectionMatrix);
            DeviceContext deviceContext = _directX3DGraphics.DeviceContext;
            _renderer.UpdateMaterialProperties(_material);
            deviceContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
            deviceContext.InputAssembler.SetVertexBuffers(0, _vertexBufferBinding);
            deviceContext.InputAssembler.SetIndexBuffer(_indexBufferObject, Format.R32_UInt, 0);
            deviceContext.DrawIndexed(_indexesCount, 0, 0);
        }

        public void Dispose()
        {
            Utilities.Dispose(ref _indexBufferObject);
            Utilities.Dispose(ref _vertexBufferObject);
        }
    }
}

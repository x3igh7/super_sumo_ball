﻿using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts
{
    class RippleTrigger : MonoBehaviour
    {
        private int[] buffer1;
        private int[] buffer2;
        private int[] vertexIndices;
        private Mesh mesh;
		private List<int> SkipList;

        private Vector3[] vertices;
        private float dampner = 0.8f;
        private float maxWaveHeight = 4.0f;
        private float MinimumCollisionMagnitude = 10.0f;

        private float forceMultiplier = 1;
        private bool swapMe = true;

        public int cols = 64;
        public int rows = 64;

        private void Start()
        {
			SkipList = new List<int>();
            mesh = GetComponent<MeshFilter>().mesh;
            vertices = mesh.vertices;

            buffer1 = new int[vertices.Length];
            buffer2 = new int[vertices.Length];

            var bounds = mesh.bounds;

            var xStep = (bounds.max.x - bounds.min.x) / cols;
            var zStep = (bounds.max.z - bounds.min.z) / rows;

            vertexIndices = new int[vertices.Length];
            int i = 0;
            for (i = 0; i < vertices.Length; i++)
            {
                vertexIndices[i] = -1;
                buffer1[i] = 0;
                buffer2[i] = 0;
            }

            for (i = 0; i < vertices.Length; i++)
            {
                float column = ((vertices[i].x - bounds.min.x) / xStep);
                float row = ((vertices[i].z - bounds.min.z) / zStep);
                float position = (row * (cols + 1)) + column + 0.5f;
                vertexIndices[(int)position] = i;
            }

            // trigger ripple at center
            //splashAtPoint(cols / 2, rows / 2);
        }

        private void Update()
        {
            Vector3[] theseVertices = new Vector3[vertices.Length];
            for (int j = 0; j < 2; j++)
            {

                int[] currentBuffer;

                // need to maintain two buffers. one trackers where the ripple was
                // the other tracks where it needs to be next.
                // by swapping between the two buffers you can create the ripple effect.
                if (swapMe)
                {
                    // process the ripples for this frame
                    processRipples(buffer1, buffer2);
                    currentBuffer = buffer2;
                }
                else
                {
                    processRipples(buffer2, buffer1);
                    currentBuffer = buffer1;
                }

                swapMe = !swapMe;

                // apply the ripples to our buffer
                int vertIndex;
                int i = 0;
                for (i = 0; i < currentBuffer.Length; i++)
                {
                    vertIndex = vertexIndices[i];
                    theseVertices[vertIndex] = vertices[vertIndex];
                    theseVertices[vertIndex].y += ((float)currentBuffer[i] / (forceMultiplier * 10)) * maxWaveHeight;
                }
            }

			SkipList = new List<int>();
			mesh.vertices = theseVertices;
            mesh.RecalculateNormals();

            var collider = GetComponent<MeshCollider>();
            collider.sharedMesh = null;
            collider.sharedMesh = mesh;
        }

        private void OnCollisionEnter(Collision collision)
        {
            var collisionMagnitude = collision.relativeVelocity.magnitude;
            if (collision.gameObject.tag == "Player" && collisionMagnitude > MinimumCollisionMagnitude)
            {
                forceMultiplier = Mathf.CeilToInt(collisionMagnitude);
                checkCollision(collision);
            }
        }

        void checkCollision(Collision collision)
        {
			print("collided!");
            var contact = collision.contacts[0];
            RaycastHit hit;
            var origin = new Vector3(0f, 20f);
            var ray = new Ray(origin, contact.point - origin);
            if (Physics.Raycast(ray, out hit))
            { 
                if(hit.collider.gameObject.tag == "Surface")
                {
                    // getting the inverse of the coordinates for an accurate hit location
                    var xTextureCoord = 1 - hit.textureCoord.x;
                    var yTextureCoord = 1 - hit.textureCoord.y;
                    
                    Bounds bounds = mesh.bounds;
                    float xStep = (bounds.max.x - bounds.min.x) / cols;
                    float zStep = (bounds.max.z - bounds.min.z) / rows;
                    float xCoord = (bounds.max.x - bounds.min.x) - ((bounds.max.x - bounds.min.x) * xTextureCoord);
                    float zCoord = (bounds.max.z - bounds.min.z) - ((bounds.max.z - bounds.min.z) * yTextureCoord);
                    float column = (xCoord / xStep);
                    float row = (zCoord / zStep);


					var collisionMagnitude = collision.relativeVelocity.magnitude;

					MonoBehaviour[] list = collision.gameObject.GetComponents<MonoBehaviour>();

					foreach (MonoBehaviour mb in list)
					{
						if (mb is PlayerController)
						{
							if (Input.GetButton((mb as PlayerController).dashButton))
							{
								print("Relative veoocity: " + collision.relativeVelocity);
								int position = (((int)row * (cols + 1)) + (int)column);

								if (collision.relativeVelocity.x > 4)
								{
									//buffer1[position] = 0;
									//buffer1[position - 1] = 0;
									//buffer1[position + 1] = 0;
									//buffer1[position + (cols + 1)] = 0;
									//buffer1[position + (cols + 1) + 1] = 0;
									//buffer1[position + (cols + 1) - 1] = 0;
									//buffer1[position - (cols + 1)] = 0;
									//buffer1[position - (cols + 1) + 1] = 0;
									//buffer1[position - (cols + 1) - 1] = 0;
								}
								else if (collision.relativeVelocity.x < -4)
								{
									
								}
								else
								{

								}
								if (collision.relativeVelocity.z > 4)
								{
									
								}
								else if(collision.relativeVelocity.z < -4)
								{
									
								}
								else
								{

								}

								if ((mb as PlayerController).Slamming)
								{
									(mb as PlayerController).Slamming = false;
								}
								splashAtPoint((int)column, (int)row, 100);
							}
							else if ((mb as PlayerController).Slamming)
							{
								(mb as PlayerController).Slamming = false;
								splashAtPoint((int)column, (int)row);
							}
						}
						if(mb is GoalBall)
						{
							splashAtPoint((int)column, (int)row, 300);
						}
					}
					

					if (swapMe)
					{
						// process the ripples for this frame
						processRipples(buffer1, buffer2);
						processRipples(buffer2, buffer1);
						processRipples(buffer1, buffer2);
						processRipples(buffer2, buffer1);
					}
					else
					{
						processRipples(buffer2, buffer1);
						processRipples(buffer1, buffer2);
						processRipples(buffer2, buffer1);
						processRipples(buffer1, buffer2);
					}


				}
            }
        }

        void splashAtPoint(int x, int y, int force = 800)
        {
			//var modifiedBaseSplash = Mathf.CeilToInt(force * forceMultiplier/32);
			int position = ((y * (cols + 1)) + x);
            buffer1[position] += force;
            buffer1[position - 1] += force;
			buffer1[position + 1] += force;
			buffer1[position + (cols + 1)] += force;
			buffer1[position + (cols + 1) + 1] += force;
			buffer1[position + (cols + 1) - 1] += force;
			buffer1[position - (cols + 1)] += force;
			buffer1[position - (cols + 1) + 1] += force;
			buffer1[position - (cols + 1) - 1] += force;
		}

        void processRipples(int[] source, int[] dest)
        {
            int x = 0;
            int y = 0;
            int position = 0;
            for (y = 1; y < rows - 1; y++)
            {
                for (x = 1; x < cols; x++)
                {
                    position = (y * (cols + 1)) + x;
                    dest[position] = (((source[position - 1] +
                                         source[position + 1] +
                                         source[position - (cols + 1)] +
                                         source[position + (cols + 1)]) >> 1) - dest[position]);
                    dest[position] = (int)(dest[position] * (dampner));
                }
            }
        }
    }
}

using UnityEngine;

namespace Mirror
{
    /// <summary>
    /// This component is used to make a gameObject a starting position for spawning player objects in multiplayer games.
    /// <para>This object's transform will be automatically registered and unregistered with the NetworkManager as a starting position.</para>
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Network/NetworkStartPosition")]
    [HelpURL("https://mirror-networking.com/docs/Components/NetworkStartPosition.html")]
    public class NetworkStartPosition : MonoBehaviour
    {
        [SerializeField] Mesh playerMesh;
        public void Awake()
        {
            NetworkManager.RegisterStartPosition(transform);
        }

        public void OnDestroy()
        {
            NetworkManager.UnRegisterStartPosition(transform);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            if (playerMesh != null)
                Gizmos.DrawMesh(playerMesh, transform.position, transform.rotation, Vector3.one * 0.01f);
        }

        [ContextMenu("Align With Floor")]
        void Floor()
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity))
            {
                transform.position = hit.point;
            }
        }
    }
}

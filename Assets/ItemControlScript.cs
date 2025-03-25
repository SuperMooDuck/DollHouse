using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemControlScript : MonoBehaviour
{
    new public Collider2D collider; //Для проверки столкновений с поверхностями
    public Collider2D visualMask; // Чтобы избавиться от перекрытия предметом мебели
    bool dragged = false; // Перемещается ли предмет игроком

    void Update()
    {
        if (dragged) return;

        // На сцене созданы объекты с одними коллайдерами, для обозначения поверхностей и стен.
        // Предмет падает пока не встретит подходящую поверхность, либо пока сталкивается со стеной, либо когда он находится на полу, но перекрывает своим спрайтом другую поверхность
        
        bool background = collider.IsTouchingLayers(LayerMask.GetMask("Background"));
        bool surface = collider.IsTouchingLayers(LayerMask.GetMask("Surface"));
        bool wall = collider.IsTouchingLayers(LayerMask.GetMask("Wall"));
        bool visualOverlapsSurface = visualMask.IsTouchingLayers(LayerMask.GetMask("Surface"));

        if ((wall && !surface) || (!background && !surface) || (!surface && visualOverlapsSurface))
        {
            var pos = transform.position;
            pos.y -= Time.deltaTime * 2f;
            if (pos.y <= InputController.BottomLimit)
                pos.y = 0;
            transform.position = pos;
        }
    }

    public void DragStart()
    {
        dragged = true;
        transform.localScale = new Vector3(1.2f, 1.2f, 1f);
    }

    public void DragEnd()
    {
        dragged = false;
        transform.localScale = new Vector3(1f, 1f, 1f);
    }
}

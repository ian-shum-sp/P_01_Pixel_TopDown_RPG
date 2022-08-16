using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private bool _isActivated = false;
    public Common.SceneName sceneName;

    private void OnTriggerEnter2D(Collider2D collider) 
    {
        if(_isActivated)
            return;

        if(collider.name == "Player")
        {
            _isActivated = true;
            GameScene scene = new GameScene();
            scene.SceneName = sceneName;
            switch(sceneName)
            {
                case Common.SceneName.DUNGEON_CENTRAL_HUB:
                {
                    scene.SceneDisplayName = "Dungeon Central Hub";
                    break;
                }
                case Common.SceneName.ENCHANTED_FOREST_CENTRAL_HUB:
                {
                    scene.SceneDisplayName = "Enchanted Forest Central Hub";
                    break;
                }
                case Common.SceneName.FANTASY_CENTRAL_HUB:
                {
                    scene.SceneDisplayName = "Fantasy Central Hub";
                    break;
                }
                case Common.SceneName.DUNGEON_ADVENTURE_MAP:
                {
                    scene.SceneDisplayName = "Deep Dungeon";
                    break;
                }
                case Common.SceneName.ENCHANTED_FOREST_ADVENTURE_MAP:
                {
                    scene.SceneDisplayName = "Enchanted Forest";
                    break;
                }
                case Common.SceneName.FANTASY_ADVENTURE_MAP:
                {
                    scene.SceneDisplayName = "Fantasy Land";
                    break;
                }
                default:
                {
                    scene.SceneDisplayName = "";
                    break;
                }
            }
            GameManager.Instance.LoadScene(scene);
        }
    }
}

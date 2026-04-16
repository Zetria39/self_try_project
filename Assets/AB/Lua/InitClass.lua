-- 导入脚本
require("LuaOOP")                   --面向对象实现
require("SplitTools")               --字符串拆分实现
Json = require("JsonUtility")       --Json解析
-- Unity常用API的全局调用变量都在这里声明
UnityEngine = CS.UnityEngine
GameObject = CS.UnityEngine.GameObject
Transform = CS.UnityEngine.Transform
Vector3 = CS.UnityEngine.Vector3
Vector2 = CS.UnityEngine.Vector2
Input = CS.UnityEngine.Input
Resources = CS.UnityEngine.Resources
TextAsset = CS.UnityEngine.TextAsset
-- 图集相关类
SpriteAtlas = CS.UnityEngine.U2D.SpriteAtlas
-- UGUI相关
RectTransform = CS.UnityEngine.RectTransform
UI = CS.UnityEngine.UI
Image = UI.Image
Text = UI.Text
Button = UI.Button
Toggle = UI.Toggle
Silder = UI.Slider
ScrollRect = UI.ScrollRect
Dropdown = UI.Dropdown
UIBahaviour = CS.UnityEngine.EventSystems.UIBehaviour
-- 框架相关
ABManager = CS.ABManager.Instance
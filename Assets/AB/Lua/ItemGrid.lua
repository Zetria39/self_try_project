-- 声明ItemGrid类
Object:subClass("ItemGrid")
-- “成员变量”
ItemGrid.obj = nil          --格子对象
ItemGrid.imgIcon = nil      --格子使用的图标
ItemGrid.Text = nil         --格子显示的数量

-- 实例化格子对象
-- 参数: 
--      father:格子父对象; poxX:x轴位置; posY:y轴位置;
function ItemGrid:Init(father, posX, posY)
    self.obj = ABManager:LoadRes("ui", "ItemGrid", typeof(GameObject))
    self.obj.transform:SetParent(father, false)
    self.obj.transform.localPosition = Vector3(posX, posY, 0)
    -- 获取格子子控件
    self.imgIcon = self.obj.transform:Find("imgIcon"):GetComponent(typeof(Image))
    self.Text = self.obj.transform:Find("txtNum"):GetComponent(typeof(Text))
end

-- 初始化格子信息
-- 参数: 
--      data:外面传入的道具信息
function ItemGrid:InitData(data)
    -- 通过 道具ID 去读取 道具配置表得到图标信息
    local itemData = ItemData[data.id]
    local strs = string.split(itemData.icon, "_")
    local spriteAtlas = ABManager:LoadRes("ui", strs[1], typeof(SpriteAtlas))
    self.imgIcon.sprite = spriteAtlas:GetSprite(strs[2])
    -- 设置数量
    self.Text.text = data.num
end

-- 删除格子
function ItemGrid:Destroy()
    GameObject.Destroy(self.obj)
    self.obj = nil
end
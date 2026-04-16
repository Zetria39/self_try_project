-- 一个面板对应一个表
BasePanel:subClass("BagPanel")
BagPanel.name = "BagPanel"

BagPanel.Content = nil
-- 存储当前显示的格子
BagPanel.items = {}
BagPanel.nowType = -1
BagPanel.isshow = false

function BagPanel:Init(name)
    self.base.Init(self, name)
    if self.isInitEvent == false then
        self.Content = self:GetControl("svBag", "ScrollRect").transform:Find("Viewport"):Find("Content")
        -- 加事件
        -- 关闭按钮
        self:GetControl("btnClose", "Button").onClick:AddListener(function()
            self:HideMe()
        end)
        -- toggle监听函数
        self:GetControl("togEquip", "Toggle").onValueChanged:AddListener(function(value)
            if value == true then
                self:ChangeType(1)
            end
        end)
        self:GetControl("togItem", "Toggle").onValueChanged:AddListener(function(value)
            if value == true then
                self:ChangeType(2)
            end
        end)
        self:GetControl("togGem", "Toggle").onValueChanged:AddListener(function(value)
            if value == true then
                self:ChangeType(3)
            end
        end)

        self.isInitEvent = true
    end
end

function BagPanel:ShowMe()
    self.base.ShowMe(self)
    self.isshow = true
    -- 第一次打开是更新数据
    if self.nowType == -1 then
        self:ChangeType(1)
    end
end

function BagPanel:HideMe()
    self.base.HideMe(self)
    self.isshow = false
end

function BagPanel:ShowORHideMe()
    if self.isshow == false then
        self:ShowMe()
    else
        self:HideMe()
    end
end

function BagPanel:ChangeType(type)
    -- 判断是否为同一页签
    if self.nowType == type then
        return
    end
    self.nowType = type
    -- 切换页面，根据玩家信息，来进行格子创建
    -- 更新之前，把老的格子删掉
    for i = 1, #self.items do
        self.items[i]:Destroy()
    end
    self.items = {}

    -- 再根据当前选择的类型，来创建新的格子
    local nowItems = nil
    if type == 1 then
        nowItems = PlayerData.equips
    elseif type == 2 then
        nowItems = PlayerData.items
    else
        nowItems = PlayerData.gems
    end
    -- 创建格子
    for i = 1, #nowItems do
        -- 实例化格子对象并设置位置，根据索引获取数据，初始化格子信息
        local grid = ItemGrid:new()
        grid:Init(self.Content, (i - 1) % 4 * 185, math.floor((i - 1) / 4) * - 185)
        grid:InitData(nowItems[i])
        table.insert(self.items, grid)
    end
end
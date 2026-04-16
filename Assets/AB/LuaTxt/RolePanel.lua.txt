-- 一个面板对应一个表
BasePanel:subClass("RolePanel")
RolePanel.name = "RolePanel"

RolePanel.Content = nil
-- 存储当前显示的格子
RolePanel.items = {}
RolePanel.nowType = -1
RolePanel.isshow = false


function RolePanel:Init(name)
    self.base.Init(self, name)
    if self.isInitEvent == false then
        -- 加事件
        -- 关闭按钮
        self:GetControl("btnClose", "Button").onClick:AddListener(function()
            self:HideMe()
        end)

        self.isInitEvent = true
    end
end

function RolePanel:ShowMe()
    self.base.ShowMe(self)
    self.isshow = true
end

function RolePanel:HideMe()
    self.base.HideMe(self)
    self.isshow = false
end


function RolePanel:ShowORHideMe()
    if self.isshow == false then
        self:ShowMe()
    else
        self:HideMe()
    end
end
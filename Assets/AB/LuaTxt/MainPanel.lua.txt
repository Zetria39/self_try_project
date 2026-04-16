BasePanel:subClass("MainPanel")
MainPanel.name = "MainPanel"
MainPanel.isshow = false

function MainPanel:Init()
    self.base.Init(self)
    -- 避免重复向UI控件添加监听事件
    if self.isInitEvent == false then
        self:GetControl("btnRole", "Button").onClick:AddListener(function()
            self:BtnRoleClick()
        end)
        self.isInitEvent = true
    end
end

function MainPanel:BtnRoleClick()
    BagPanel:ShowMe()
    RolePanel:ShowMe()
end


function MainPanel:ShowMe()
    self.base.ShowMe(self)
    self.isshow = true
end

function MainPanel:HideMe()
    self.base.HideMe(self)
    self.isshow = false
end

function MainPanel:ShowORHideMe()
    if self.isshow == false then
        self:ShowMe()
    else
        self:HideMe()
    end
end
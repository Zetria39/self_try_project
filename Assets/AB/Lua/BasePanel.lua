Object:subClass("BasePanel")
BasePanel.panelObj = nil
BasePanel.name = nil
-- 控件字典，键为对象名，值为UI控件脚本
-- 最终存储形式
-- { btnRole = { Image = 控件, Button = 控件 } 
--   togItem = { Toggle = 控件 } }
BasePanel.controls = {}
BasePanel.isInitEvent = false

function BasePanel:Init()
    if self.panelObj == nil then
        -- 实例化面板对象
        self.panelObj = ABManager:LoadRes("ui", self.name, typeof(GameObject))
        self.panelObj.transform:SetParent(Canvas, false)
        -- 获取面板下所有的UI控件
        local allControls = self.panelObj:GetComponentsInChildren(typeof(UIBahaviour))
        for i = 0, allControls.Length - 1 do
            local controlName = allControls[i].name
            -- 如果对象名字前添加了符合规定的前缀，就将该对象的控件加入到表内
            if string.find(controlName, "btn") ~= nil or
               string.find(controlName, "tog") ~= nil or
               string.find(controlName, "img") ~= nil or
               string.find(controlName, "sv") ~= nil or
               string.find(controlName, "txt") ~= nil then
                -- 为了让我们在获取控件时，能够确定得到得控件的类型，因此需要存储类型，我们通过反射获取类名
                local typeName = allControls[i]:GetType().Name
                -- 因为有可能一个对象上会挂载多个UI控件脚本，因此让控件字典存储的值为表，将这些UI控件脚本都放到表里
                if self.controls[allControls[i].name] ~= nil then
                    -- 通过自定义索引的方式，存储一个新的控件，索引为类型名
                    self.controls[allControls[i].name][typeName] = allControls[i]
                else
                    self.controls[allControls[i].name] = {[typeName] = allControls[i]}
                end
            end
        end
    end -- if self.panelObj == nil
end

function BasePanel:ShowMe()
    self:Init()
    self.panelObj:SetActive(true)
end

function BasePanel:HideMe()
    self.panelObj:SetActive(false)
end

-- 得到控件
-- 参数: name:控件依附对象的名字; typeName:控件的类型名
function BasePanel:GetControl(name, typeName)
    if self.controls[name] ~= nil then
        local sameNameControls = self.controls[name]
        if sameNameControls[typeName] ~= nil then
            return sameNameControls[typeName]
        end
    end
    return nil
end
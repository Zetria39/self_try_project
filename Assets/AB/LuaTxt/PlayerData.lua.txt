PlayerData = {}
-- 目前只做背包功能，所以只需要它的道具信息即可
PlayerData.equips = {}
PlayerData.items = {}
PlayerData.gems = {}

-- 为玩家数据写了一个初始化方法，以后直接改这里的数据来源即可
function PlayerData:Init()
    -- 假设我们在这里所以本地加载或者是服务器下载得到了玩家信息
    table.insert(self.equips, {id = 1, num = 1})
    table.insert(self.equips, {id = 2, num = 1})
    
    table.insert(self.items, {id = 3, num = 50})
    table.insert(self.items, {id = 4, num = 20})
    
    table.insert(self.gems, {id = 5, num = 99})
    table.insert(self.gems, {id = 6, num = 88})
end

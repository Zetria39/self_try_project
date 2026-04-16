-- 加载AB包中的ItemData，Json文件
local txt = ABManager:LoadRes("json", "ItemData", typeof(TextAsset))
print(txt.text)
-- 解析Json文件，得到一个类似于数组结构的数据
local itemList = Json.decode(txt.text)
-- 数组结构数据不方便我们通过id获取信息，因此使用新表转存一次，并使用全局变量存储
ItemData = {}
for _, value in pairs(itemList) do
    ItemData[value.id] = value
end
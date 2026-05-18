using System;
using System.Collections.Generic;

[Serializable]
public class MachineData
{
    public string machineId;
    public string status;
    public string updatedAt;
    public List<ProductData> products;
}
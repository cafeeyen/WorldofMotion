using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;

/*
 * Original Source
 * From : http://wiki.unity3d.com/index.php?title=Save_and_Load_from_XML
 * Author : Zumwalt
*/

public class SaveLoad : MonoBehaviour
{
    // An example where the encoding can be found is at 
    // http://www.eggheadcafe.com/articles/system.xml.xmlserialization.asp 
    // We will just use the KISS method and cheat a little and use 
    // the examples from the web page since they are fully described 

    // Set prefebs of each item type
    public GameObject cube, sphere;
    public GameObject WorldObject;
    public ItemObjectController ItemCon;
    public UIController UICon;

    private string _FileLocation, _FileName;
    private UserData myData;
    private string _data;
    private GameObject itemObject;

    void Start()
    {
        _FileLocation = Application.persistentDataPath;
        myData = new UserData();

        if (PlayerPrefs.GetInt("World") != 0)
        {
            _FileName = "World" + PlayerPrefs.GetInt("World") + ".xml"; ;
            LoadWorld();
        }
    }

    void SaveWorld()
    {
        myData._iWorld = new List<UserData.WorldData>();
        foreach (GameObject item in GetComponent<WorldObject>().getItemList())
        {
            UserData.WorldData data = new UserData.WorldData();
            data.itemType = item.GetComponent<ItemObject>().ItemType;
            data.pos = item.transform.position;
            data.scale = item.transform.localScale;
            data.rot = item.transform.localRotation;
            data.velocity = item.GetComponent<ItemObject>().Velocity;
            data.gravity = item.GetComponent<ItemObject>().IsGravity;
            data.surType = item.GetComponent<ItemObject>().getSurType().getName();
            myData._iWorld.Add(data);
        }
        // Time to create our XML! 
        _data = SerializeObject(myData);
        // This is the final resulting XML from the serialization process
        if (PlayerPrefs.GetInt("World") == 0)
            PlayerPrefs.SetInt("World", 1);
        _FileName = "World" + PlayerPrefs.GetInt("World") + ".xml";
        CreateXML();
        PlayerPrefs.SetInt("HaveWorldSaved", 1);
    }

    public void saveChk()
    {
        itemObject = ItemCon.getCurrentObject();
        if(itemObject == null || !itemObject.GetComponent<ItemObject>().IsOverlap)
        {
            if (PlayerPrefs.GetInt("HaveWorldSaved") == 1)
            {
                UICon.playSound("clk");
                UICon.setAlert(true);
            }
            else
            {
                UICon.playSound("clk");
                SaveWorld();
            }
        }
        else
        {
            UICon.playSound("deny");
            UICon.setDeny(true);
        }
    }

    public void answerChk(bool ans)
    {
        if (ans)
            SaveWorld();
        UICon.playSound("clk");
        UICon.setAlert(false);
    }

    public void LoadWorld()
    {
        // Load our UserData into myData 
        LoadXML();
        if (_data.ToString() != "")
        {
            // notice how I use a reference to type (UserData) here, you need this 
            // so that the returned object is converted into the correct type 
            myData = (UserData)DeserializeObject(_data);
            // set the players position to the data we loaded 
            //GetComponent<WorldObject>().setItemList(myData._iWorld.itemList);
            // just a way to show that we loaded in ok 
            WorldObject = GameObject.Find("WorldObject");
            var itemCon = GameObject.Find("ItemObjectController").GetComponent<ItemObjectController>();
            foreach (UserData.WorldData data in myData._iWorld)
            {
                GameObject itemObject;
                switch(data.itemType)
                {
                    case "Cube":
                        itemObject = (GameObject)Instantiate(cube, data.pos, data.rot);
                        break;
                    case "Sphere":
                        itemObject = (GameObject)Instantiate(sphere, data.pos, data.rot);
                        break;
                    default :
                        itemObject = new GameObject();
                        Debug.Log("Naniii");
                        break;
                }
                itemObject.transform.parent = WorldObject.transform;
                itemObject.transform.localScale = data.scale;
                itemObject.GetComponent<ItemObject>().Velocity = data.velocity;
                itemObject.GetComponent<ItemObject>().IsGravity = data.gravity;
                itemObject.GetComponent<ItemObject>().ItemType = data.itemType;
                itemCon.setItemObject(itemObject);
                itemObject.GetComponent<ItemObject>().setSurType(data.surType);
                // Cancle select
                itemCon.setItemObject(itemObject);
            }
            Debug.Log("Loaded");
        }
    }

    /* The following metods came from the referenced URL */
    string UTF8ByteArrayToString(byte[] characters)
    {
        UTF8Encoding encoding = new UTF8Encoding();
        string constructedString = encoding.GetString(characters);
        return (constructedString);
    }

    byte[] StringToUTF8ByteArray(string pXmlString)
    {
        UTF8Encoding encoding = new UTF8Encoding();
        byte[] byteArray = encoding.GetBytes(pXmlString);
        return byteArray;
    }

    // Here we serialize our UserData object of myData 
    string SerializeObject(object pObject)
    {
        string XmlizedString = null;
        MemoryStream memoryStream = new MemoryStream();
        XmlSerializer xs = new XmlSerializer(typeof(UserData));
        XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
        xs.Serialize(xmlTextWriter, pObject);
        memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
        XmlizedString = UTF8ByteArrayToString(memoryStream.ToArray());
        return XmlizedString;
    }

    // Here we deserialize it back into its original form 
    object DeserializeObject(string pXmlizedString)
    {
        XmlSerializer xs = new XmlSerializer(typeof(UserData));
        MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(pXmlizedString));
        return xs.Deserialize(memoryStream);
    }

    // Finally our save and load methods for the file itself 
    void CreateXML()
    {
        StreamWriter writer;
        FileInfo t = new FileInfo(_FileLocation + "/" + _FileName);
        if (!t.Exists)
        {
            writer = t.CreateText();
        }
        else
        {
            t.Delete();
            writer = t.CreateText();
        }
        writer.Write(_data);
        writer.Close();
        Debug.Log("File written.");
    }

    void LoadXML()
    {
        StreamReader r = File.OpenText(_FileLocation + "/" + _FileName);
        string _info = r.ReadToEnd();
        r.Close();
        _data = _info;
        Debug.Log("File Read");
    }
}

// UserData is our custom class that holds our defined objects we want to store in XML format 
public class UserData
{
    // We have to define a default instance of the structure 
    public List<WorldData> _iWorld;
    // Default constructor doesn't really do anything at the moment 
    public UserData() { }

    // Anything we want to store in the XML file, we define it here 
    public struct WorldData
    {
        public Vector3 pos, scale, velocity;
        public Quaternion rot;
        public string itemType;
        public bool gravity;
        public string surType;
    }
}
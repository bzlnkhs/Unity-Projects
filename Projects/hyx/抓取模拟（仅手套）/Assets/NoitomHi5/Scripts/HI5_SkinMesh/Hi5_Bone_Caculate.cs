using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HI5;
using System.IO;
using System.Xml;
public class Hi5_Bone_Caculate : MonoBehaviour
{
    public string filename = "";
    public string Id = "";
    public Transform[] mbones;
    public Transform[] testpoints;
    public Transform[] handpoints;
    public Transform[] targetPoints;
    private static string m_Path = "C:\\ProgramData\\Noitom\\hi5_sdk\\glovebonefile";
    float[] ratio_thumbs = new float[2] { 0.593f, 0.407f };
    float[] ratio_indexs = new float[3] { 0.510f, 0.287f, 0.203f };
    float[] ratio_middles = new float[3] { 0.505f, 0.298f, 0.197f };
    float[] ratio_rings = new float[3] { 0.491f, 0.304f, 0.205f };
    float[] ratio_pinkys = new float[3] { 0.490f, 0.271f, 0.239f };

    //指头内部分摊
    float ratio_pinky_inhand = 0.4329f;
    float ratio_ring_inhand = 0.4208f;
    float ratio_middle_inhand = 0.3953f;
    float ratio_index_inhand = 0.3822f;
    float ratio_thumb_inhand = 0.2500f;
    void OnEnable()
    {
        ReadLengthXML();
    }
    public void ReadLengthXML()
    {
        if (File.Exists(m_Path + "\\" + "meanture_" + Id + ".xml"))
        {
            Debug.Log("ReadLengthXML");
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(m_Path + "\\" + "meanture_" + Id + ".xml");
            XmlNodeList root = xmlDoc.SelectSingleNode("LeapMotionGlove").ChildNodes;
            foreach (XmlNode xn in root)
            {
                if (string.Compare(xn.Attributes["GloveSide"].Value, "GM_LeftGlove") == 0)
                {
                    List<float> InArray = new List<float>();
                    foreach (XmlNode positionNode in xn)
                    {
                       float value = float.Parse(positionNode.Attributes["length"].Value)/100.0f;
                       InArray.Add(value);
                    }
                    List<Vector3> ModelTransformArray = new List<Vector3>();
                    for (int i = 0; i < mbones.Length; i++)
                    {
                        ModelTransformArray.Add(mbones[i].position);
                    }
                    Dictionary<int, Vector3>  hands = CaculateHandPosition(InArray, ModelTransformArray,true);
                    foreach (KeyValuePair<int, Vector3> item in hands)
                    {
                        //if(item.Key >4 && item.Key < 10)
                        //if (item.Key < 20 )
                         mbones[item.Key].localPosition = item.Value;
                        //targetPoints[item.Key].localPosition = item.Value;
                    }
                }
            }
        }
    }
    public void ReadXml()
    {
        if (File.Exists(m_Path + "\\" + filename + ".xml"))
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(m_Path + "\\" + filename + ".xml");
            XmlNodeList root = xmlDoc.SelectSingleNode("LeapMotionGlove").ChildNodes;
            foreach (XmlNode xn in root)
            {
                if (string.Compare(xn.Attributes["GloveSide"].Value, "GM_LeftGlove") == 0)
                {
                    List<Vector3> points = new List<Vector3>();
                    foreach (XmlNode positionNode in xn)
                    {
                        Vector3 pos = new Vector3();
                        pos.x = float.Parse(positionNode.Attributes["X"].Value);
                        pos.y = float.Parse(positionNode.Attributes["Y"].Value);
                        pos.z = float.Parse(positionNode.Attributes["Z"].Value);
                        points.Add(pos);
                    }
                }
                if (string.Compare(xn.Attributes["GloveSide"].Value, "GM_RightGlove") == 0)
                {
                    List<Vector3> points = new List<Vector3>();
                    foreach (XmlNode positionNode in xn)
                    {
                        Vector3 pos = new Vector3();
                        pos.x = float.Parse(positionNode.Attributes["X"].Value);
                        pos.y = float.Parse(positionNode.Attributes["Y"].Value);
                        pos.z = float.Parse(positionNode.Attributes["Z"].Value);
                        points.Add(pos);
                    }
                }
            }
        }

    }
    /// <summary>
    /// 根据骨骼之间长度相对模型向量计算输出算法11点偏移量
    /// </summary>
    /// <param name="InArray"> 读取配置文件距离偏移量  </param>
    /// <param name="ModelTransformArray">  美术模型25点原始骨节点位置</param>
    /// <param name="measureVector3">算法11节点</param>
    private void DistanceChangetoVector(List<float> InArray,List<Vector3> ModelTransformArray,
                                        Dictionary<int,Vector3> MeasureVector3,
                                        bool isLeft)
    {
        //先给定算法根节点初始值
        MeasureVector3.Add(10, new Vector3(0.0f, 0.0f, 0.0f));
        testpoints[10].position = MeasureVector3[10];
        //求大拇指根据测量距离调整骨骼长度后新位置
        //求大拇指第二骨节点算法位置 算法下标1
        Vector3 positionModeZero = ModelTransformArray[0];   //模型手根节点位置
        Vector3 thumbModeTwo = ModelTransformArray[2];   //模型拇指第二根节点位置
        Vector3 offsetTwoThumb = (thumbModeTwo - positionModeZero).normalized*InArray[0];
        MeasureVector3.Add(1, offsetTwoThumb);
        testpoints[1].position = testpoints[10].position+MeasureVector3[1];
        //求大拇指第一骨节点算法位置 算法下标0
        Vector3 thumbModeFour = ModelTransformArray[4];
        Vector3 offsetFourThumb = (thumbModeFour - thumbModeTwo).normalized * InArray[1];
        MeasureVector3.Add(0, offsetFourThumb);
        testpoints[0].position = testpoints[10].position + MeasureVector3[1]+ MeasureVector3[0];

        //求Index根据测量距离调整骨骼长度后新位置
        //求Index第二骨节点算法位置 算法下标3
        Vector3 indexModeTwo = ModelTransformArray[6];
        Vector3 offsetTwoIndex = (indexModeTwo - positionModeZero).normalized * InArray[2];
        MeasureVector3.Add(3, offsetTwoIndex);
        testpoints[3].position = testpoints[10].position + MeasureVector3[3];
        //求IndexTail骨节点算法位置 算法下标2
        Vector3 indexMode9 = ModelTransformArray[9];
        Vector3 offset9Index = (indexMode9 - indexModeTwo).normalized * InArray[3];
        MeasureVector3.Add(2, offset9Index);
        testpoints[2].position = testpoints[10].position + MeasureVector3[3]+ MeasureVector3[2];

        //求Middle根据测量距离调整骨骼长度后新位置
        //求Middle第二骨节点算法位置 算法下标5
        Vector3 middleModel11 = ModelTransformArray[11];
        Vector3 offsetOneMiddle = (middleModel11 - positionModeZero).normalized * InArray[4];
        MeasureVector3.Add(5, offsetOneMiddle);
        testpoints[5].position = testpoints[10].position + MeasureVector3[5];
        //求MiddleTail骨节点算法位置 算法下标4
        Vector3 middleModel14 = ModelTransformArray[14];
        Vector3 offsetTwoMiddle = (middleModel14 - middleModel11).normalized * InArray[5];
        MeasureVector3.Add(4, offsetTwoMiddle);
        testpoints[4].position = testpoints[10].position + MeasureVector3[5] + MeasureVector3[4];
        //求Ring根据测量距离调整骨骼长度后新位置
        //求Ring第二骨节点算法位置 算法下标7
        Vector3 ringModel16 = ModelTransformArray[16];
        Vector3 offsetOneRing = (ringModel16 - positionModeZero).normalized * InArray[6];
        MeasureVector3.Add(7, offsetOneRing);
        testpoints[7].position = testpoints[10].position + MeasureVector3[7];
        //求RingTail骨节点算法位置 算法下标6
        Vector3 ringModel19 = ModelTransformArray[19];
        Vector3 offsetTwoRing = (ringModel19 - ringModel16).normalized * InArray[7];
        MeasureVector3.Add(6, offsetTwoRing);
        testpoints[6].position = testpoints[10].position + MeasureVector3[7]+ MeasureVector3[6];


        //求Pink根据测量距离调整骨骼长度后新位置
        //求Pink第二骨节点算法位置 算法下标9
        Vector3 pinkModel21 = ModelTransformArray[21];
        Vector3 offsetOnePink = (pinkModel21 - positionModeZero).normalized * InArray[8];
        MeasureVector3.Add(9, offsetOnePink);
        testpoints[9].position = testpoints[10].position + MeasureVector3[9];
        //求RingTail骨节点算法位置 算法下标8
        Vector3 pinkModel24 = ModelTransformArray[24];
        Vector3 offsetTwoPink = (pinkModel24 - pinkModel21).normalized * InArray[9];
        MeasureVector3.Add(8, offsetTwoPink);
        testpoints[8].position = testpoints[10].position + MeasureVector3[9] + MeasureVector3[8];
    }

    private Dictionary<int, Vector3> CaculateHandPosition(List<float> InArray, List<Vector3> ModelTransformArray,bool isleft)
    {
       //根据模型和偏移量计算出算法所需要算法偏移点
        Dictionary<int, Vector3> MeasureVector3 = new Dictionary<int, Vector3>();
        DistanceChangetoVector(InArray, ModelTransformArray, MeasureVector3, isleft);
        Debug.Log("CaculateHandPosition");
        //写文件输出
        //计算插值其余点
        Dictionary<int, Vector3>  handpositions = LerpHandPosition(ModelTransformArray, MeasureVector3);
        return handpositions;
    }
    private Dictionary<int, Vector3> LerpHandPosition(List<Vector3> ModelTransformArray, Dictionary<int, Vector3> MeasureVector3)
    {
        Dictionary<int, Vector3> handPosition = new Dictionary<int, Vector3>();

        handPosition.Add(0, MeasureVector3[10]);
        handpoints[0].position = MeasureVector3[10];
        //计算大拇指分摊
        Vector3 thumbMeasure1to0 = MeasureVector3[0];
        Vector3 handRootTo1 = MeasureVector3[1];
        Vector3 thumb1 = handRootTo1-thumbMeasure1to0.normalized * Vector3.Dot(handRootTo1, thumbMeasure1to0.normalized) * (1-ratio_thumb_inhand);
        handPosition.Add(1, thumb1);
        handpoints[1].position = handpoints[0].position+ handPosition[1];
        Vector3 thumb2 = MeasureVector3[1]- thumb1;
        handPosition.Add(2, thumb2);
        handpoints[2].position = handpoints[1].position + handPosition[2];
        Vector3 thumb3 = thumbMeasure1to0 * ratio_thumbs[0];
        handPosition.Add(3, thumb3);
        handpoints[3].position = handpoints[2].position + handPosition[3];
        Vector3 thumb4 = thumbMeasure1to0 * ratio_thumbs[1];
        handPosition.Add(4, thumb4);
        handpoints[4].position = handpoints[3].position + handPosition[4];

        //计算index分摊
        Vector3 indexMeasure3to2 = MeasureVector3[2];
        Vector3 handRootTo3 = MeasureVector3[3];
        Vector3 index1 = handRootTo3-indexMeasure3to2.normalized * Vector3.Dot(handRootTo3, indexMeasure3to2.normalized) * (1-ratio_index_inhand);
        handPosition.Add(5, index1);
        handpoints[5].position = handpoints[0].position + handPosition[5];
        Vector3 index2 = MeasureVector3[3]- index1;
        handPosition.Add(6, index2);
        handpoints[6].position = handpoints[5].position + handPosition[6];
        Vector3 index3 = indexMeasure3to2 * ratio_indexs[0];
        handPosition.Add(7, index3);
        handpoints[7].position = handpoints[6].position + handPosition[7];
        Vector3 index4 = indexMeasure3to2 * ratio_indexs[1];
        handPosition.Add(8, index4);
        handpoints[8].position = handpoints[7].position + handPosition[8];
        Vector3 index5 = indexMeasure3to2 * ratio_indexs[2];
        handPosition.Add(9, index5);
        handpoints[9].position = handpoints[8].position + handPosition[9];

        //计算Middle
        Vector3 middleMeasure5to4 = MeasureVector3[4];
        Vector3 handRootTo5 = MeasureVector3[5];
        Vector3 middle1 = handRootTo5-middleMeasure5to4.normalized * Vector3.Dot(handRootTo5, middleMeasure5to4.normalized) *(1- ratio_middle_inhand);
        handPosition.Add(10, middle1);
        handpoints[10].position = handpoints[0].position + handPosition[10];
        Vector3 middle2 = MeasureVector3[5]- middle1;
        handPosition.Add(11, middle2);
        handpoints[11].position = handpoints[10].position + handPosition[11];
        Vector3 middle3 = middleMeasure5to4 * ratio_middles[0];
        handPosition.Add(12, middle3);
        handpoints[12].position = handpoints[11].position + handPosition[12];
        Vector3 middle4 = middleMeasure5to4 * ratio_middles[1];
        handPosition.Add(13, middle4);
        handpoints[13].position = handpoints[12].position + handPosition[13];
        Vector3 middle5 = middleMeasure5to4 * ratio_middles[2];
        handPosition.Add(14, middle5);
        handpoints[14].position = handpoints[13].position + handPosition[14];

        //计算ring
        Vector3 ringMeasure7to6 = MeasureVector3[6];
        Vector3 handRootTo7 = MeasureVector3[7];
        Vector3 ring1 = handRootTo7 - ringMeasure7to6.normalized * Vector3.Dot(handRootTo7, ringMeasure7to6.normalized) * (1-ratio_ring_inhand);
        handPosition.Add(15, ring1);
        handpoints[15].position = handpoints[0].position + handPosition[15];
        Vector3 ring2 = MeasureVector3[7]- ring1;
        handPosition.Add(16, ring2);
        handpoints[16].position = handpoints[15].position + handPosition[16];
        Vector3 ring3 = ringMeasure7to6 * ratio_rings[0];
        handPosition.Add(17, ring3);
        handpoints[17].position = handpoints[16].position + handPosition[17];
        Vector3 ring4 = ringMeasure7to6 * ratio_rings[1];
        handPosition.Add(18, ring4);
        handpoints[18].position = handpoints[17].position + handPosition[18];
        Vector3 ring5 = ringMeasure7to6 * ratio_rings[2];
        handPosition.Add(19, ring5);
        handpoints[19].position = handpoints[18].position + handPosition[19];

        //计算pinky
        Vector3 pinkyMeasure9to8 = MeasureVector3[8];
        Vector3 handRootTo9 = MeasureVector3[9];
        Vector3 pinky1 = handRootTo9-pinkyMeasure9to8.normalized * Vector3.Dot(handRootTo9, pinkyMeasure9to8.normalized) *(1- ratio_pinky_inhand);
        handPosition.Add(20, pinky1);
        handpoints[20].position = handpoints[0].position + handPosition[20];
        Vector3 pinky2 = MeasureVector3[9]- pinky1;
        handPosition.Add(21, pinky2);
        handpoints[21].position = handpoints[20].position + handPosition[21];
        Vector3 pinky3 = pinkyMeasure9to8 * ratio_pinkys[0];
        handPosition.Add(22, pinky3);
        handpoints[22].position = handpoints[21].position + handPosition[22];
        Vector3 pinky4 = pinkyMeasure9to8 * ratio_pinkys[1];
        handPosition.Add(23, pinky4);
        handpoints[23].position = handpoints[22].position + handPosition[23];
        Vector3 pinky5 = pinkyMeasure9to8 * ratio_pinkys[2];
        handPosition.Add(24, pinky5);
        handpoints[24].position = handpoints[23].position + handPosition[24];
        return handPosition;
    }


}
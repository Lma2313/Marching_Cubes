using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.IK;



public class Main : MonoBehaviour{

    public Rigidbody2D Circulo;

    private float timer = 0.0f;
    private float interval = 0.1f;
    [SerializeField] float x_offset;
    [SerializeField] float y_offset;
    [SerializeField] private float Resolution;

    [SerializeField] float Fuerza_R;
    [SerializeField] float Fuerza_X;
    [SerializeField] float Fuerza_Y;
    
    private int Max_Width = 35;
    private int Max_Heigh = 20;
    [SerializeField] private float X;
    [SerializeField] private float Y;
    [SerializeField] private float PN;
    
    [SerializeField] private float Radio;
    [SerializeField] private float PN_Scale;
    
    
    float[,] Generate_Red(float x_offset, float y_offset){
        float[,] weights = new float[Max_Width * 2 + 1, Max_Heigh * 2 + 1];
        for(int i = 0; i < Max_Width * 2 + 1; i++){
            for(int j = 0; j < Max_Heigh * 2 + 1; j++){
                weights[i,j] = PerlinNoise((float)i,(float)j,x_offset,y_offset);
            }
        }
        return weights;
    }    

    float PerlinNoise(float i, float j, float x_offset, float y_offset){
        float xCoord = i / (float)Max_Width * PN_Scale;
        float yCoord = j / (float)Max_Heigh * PN_Scale;
        return Mathf.PerlinNoise(xCoord + x_offset, yCoord + y_offset);
    }

    float Num_Random(){
        return Random.Range(0f, 1f);
    }


    void DrawCircle(float x, float y, float colour,float radius){
        Rigidbody2D Circle = Instantiate(Circulo, new Vector2(x,y), transform.rotation);
        Color grayscaleColor = Color.Lerp(Color.white, Color.black, colour);
        Circle.GetComponent<SpriteRenderer>().color = grayscaleColor;
        Circle.transform.localScale = new Vector3(radius, radius, 1);
    }

    void Draw_Red(float[,] weights){
        for (float x = -Max_Width; x < Max_Width + Resolution; x += Resolution){
            int i = Mathf.FloorToInt((x + Max_Width) / Resolution);
            for (float y = -Max_Heigh; y < Max_Heigh + Resolution; y += Resolution){
                int j = Mathf.FloorToInt((y + Max_Heigh) / Resolution);
                if (i < weights.GetLength(0) && j < weights.GetLength(1)) {
                    DrawCircle(x, y, weights[i, j], Radio);
                }
            }
        }
    }

    void Compose_IsoLanes(float[,] weights,float Delta){
        for(int i = 0; i < Max_Width * 2; i++){
            for(int j = 0; j < Max_Heigh * 2; j++){
                Draw_lines(weights[i,j + 1],weights[i + 1,j + 1],weights[i + 1,j],weights[i,j],i,j,Delta);
            }
        }
    }

    void Draw_lines(float a, float b, float c,float d, int i, int j, float Delta){

        a -= 0.5f;
        b -= 0.5f;
        c -= 0.5f;
        d -= 0.5f;

        float pm_ab = -a/(b-a);
        float pm_dc = -d/(c-d);
        float pm_da = -d/(a-d);
        float pm_cb = -c/(b-c);
        
        

        int val = 0;

        if(0f < a && a < 1f){
            val += 1;
        }
        if(0f < b && b < 1f){
            val += 2;
        }
        if(0f < c && c < 1f){
            val += 4;
        }
        if(0f < d && d < 1f){
            val += 8;
        }

        switch(val){
            case 1:
                Debug.DrawLine(new Vector2 (-Max_Width + (i + 0)*Resolution,-Max_Heigh + (j + pm_da)*Resolution), new Vector2 (-Max_Width + (i + pm_ab)*Resolution,-Max_Heigh + (j + 1)*Resolution), Color.black,Delta);
                break;
            case 2:
                Debug.DrawLine(new Vector2 (-Max_Width + (i + pm_ab)*Resolution,-Max_Heigh + (j + 1)*Resolution), new Vector2 (-Max_Width + (i + 1)*Resolution,-Max_Heigh + (j + pm_cb)*Resolution), Color.black, Delta);
                break;
            case 3:
                Debug.DrawLine(new Vector2 (-Max_Width + (i + 0)*Resolution,-Max_Heigh + (j + pm_da)*Resolution), new Vector2 (-Max_Width + (i + 1)*Resolution,-Max_Heigh + (j + pm_cb)*Resolution), Color.black, Delta);
                break;
            case 4:
                Debug.DrawLine(new Vector2 (-Max_Width + (i + pm_dc)*Resolution,-Max_Heigh + (j + 0)*Resolution), new Vector2 (-Max_Width + (i + 1)*Resolution,-Max_Heigh + (j + pm_cb)*Resolution), Color.black, Delta);
                break;
            case 5:
                Debug.DrawLine(new Vector2 (-Max_Width + (i + 0)*Resolution,-Max_Heigh + (j + pm_da)*Resolution), new Vector2 (-Max_Width + (i + pm_ab)*Resolution,-Max_Heigh + (j + 1)*Resolution), Color.black, Delta);
                Debug.DrawLine(new Vector2 (-Max_Width + (i + pm_dc)*Resolution,-Max_Heigh + (j + 0)*Resolution), new Vector2 (-Max_Width + (i + 1)*Resolution,-Max_Heigh + (j + pm_cb)*Resolution), Color.black, Delta);
                break;
            case 6:
                Debug.DrawLine(new Vector2 (-Max_Width + (i + pm_dc)*Resolution,-Max_Heigh + (j + 0)*Resolution), new Vector2 (-Max_Width + (i + pm_ab)*Resolution,-Max_Heigh + (j + 1)*Resolution), Color.black, Delta);
                break;
            case 7:
                Debug.DrawLine(new Vector2 (-Max_Width + (i + 0)*Resolution,-Max_Heigh + (j + pm_da)*Resolution), new Vector2 (-Max_Width + (i + pm_dc)*Resolution,-Max_Heigh + (j + 0)*Resolution), Color.black, Delta);
                break;
            case 8:
                Debug.DrawLine(new Vector2 (-Max_Width + (i + 0)*Resolution,-Max_Heigh + (j + pm_da)*Resolution), new Vector2 (-Max_Width + (i + pm_dc)*Resolution,-Max_Heigh + (j + 0)*Resolution), Color.black, Delta);
                break;
            case 9:
                Debug.DrawLine(new Vector2 (-Max_Width + (i + pm_dc)*Resolution,-Max_Heigh + (j + 0)*Resolution), new Vector2 (-Max_Width + (i + pm_ab)*Resolution,-Max_Heigh + (j + 1)*Resolution), Color.black, Delta);
                break;
            case 10:
                Debug.DrawLine(new Vector2 (-Max_Width + (i + 0)*Resolution,-Max_Heigh + (j + pm_da)*Resolution), new Vector2 (-Max_Width + (i + pm_ab)*Resolution,-Max_Heigh + (j + 1)*Resolution), Color.black, Delta);
                Debug.DrawLine(new Vector2 (-Max_Width + (i + pm_dc)*Resolution,-Max_Heigh + (j + 0)*Resolution), new Vector2 (-Max_Width + (i + 1)*Resolution,-Max_Heigh + (j + pm_cb)*Resolution), Color.black, Delta);
                break;
            case 11:
                Debug.DrawLine(new Vector2 (-Max_Width + (i + pm_dc)*Resolution,-Max_Heigh + (j + 0)*Resolution), new Vector2 (-Max_Width + (i + 1)*Resolution,-Max_Heigh + (j + pm_cb)*Resolution), Color.black, Delta);
                break;
            case 12:
                Debug.DrawLine(new Vector2 (-Max_Width + (i + 0)*Resolution,-Max_Heigh + (j + pm_da)*Resolution), new Vector2 (-Max_Width + (i + 1)*Resolution,-Max_Heigh + (j + pm_cb)*Resolution), Color.black, Delta);
                break;
            case 13:
                Debug.DrawLine(new Vector2 (-Max_Width + (i + 1)*Resolution,-Max_Heigh + (j + pm_cb)*Resolution), new Vector2 (-Max_Width + (i + pm_ab)*Resolution,-Max_Heigh + (j + 1)*Resolution), Color.black, Delta);
                break;
            case 14:
                Debug.DrawLine(new Vector2 (-Max_Width + (i + 0)*Resolution,-Max_Heigh + (j + pm_da)*Resolution), new Vector2 (-Max_Width + (i + pm_ab)*Resolution,-Max_Heigh + (j + 1)*Resolution), Color.black, Delta);
                break;
            default:
                break;
        }
    }

    void Start(){
        PN_Scale = 4.0f;
        x_offset = 0f;
        y_offset = 0f;
        Resolution = 1f;
        
        if(Radio < 0.1f)
            Radio = 0.5f;

        Fuerza_R = 0.01f;
        Fuerza_X = 0.01f;        
        Fuerza_Y = 0.01f;
    }
    void Rewrite_Red(float x_offset, float y_offset){
        float[,] pesos;
        pesos = Generate_Red(x_offset,y_offset);
        //Draw_Red(pesos);
        
        Compose_IsoLanes(pesos,interval);
    }
    private void Update(){

        float ejeHorizontal = Input.GetAxisRaw("Horizontal")*Fuerza_X;
        float ejeVertical = Input.GetAxisRaw("Vertical")*Fuerza_Y;
        
        float ejeImaginario = 0;
        if (Input.GetKey(KeyCode.Q))
        {
            ejeImaginario = Fuerza_R;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            ejeImaginario = -Fuerza_R;
        }

        Resolution += ejeImaginario;
        x_offset += ejeHorizontal/Resolution;
        y_offset += ejeVertical/Resolution;
        
        timer += Time.deltaTime;
        if(timer>= interval){
            Rewrite_Red(x_offset,y_offset);
            timer -= interval;
        }
        
    }
}
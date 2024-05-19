Shader "Transparent/Diffuse ZWrite" {
    Properties{
        _Color("Main Color", Color) = (1,1,1,1)
    }
    SubShader{
        Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
        LOD 200

        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            Stencil {
                Ref 10
                ReadMask 10
                Comp NotEqual
                Pass Replace
            }

        }

        
        // paste in forward rendering passes from Transparent/Diffuse
        // UsePass "Transparent/Diffuse/FORWARD"
    }
    Fallback "Transparent/VertexLit"
}

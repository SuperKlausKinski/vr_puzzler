Shader "Cube Lighting/Diffuse" {
	Properties{
		_MainTex("Main Texture", 2D) = "white" {}
	_DiffuseCube("Diff cube", Cube) = "" { TexGen CubeNormal }
	}
		SubShader{
		Pass{
		SetTexture[_MainTex]{ combine texture }
		SetTexture[_DiffuseCube]{ matrix[_ViewToWorld] combine previous * texture DOUBLE, previous }
	}
	}
		SubShader{
		Pass{
		SetTexture[_MainTex]{
		Combine texture
	}
	}
	}
}
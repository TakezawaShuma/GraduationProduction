//
// Transform.cginc
// 
// Author : Tama
//
// UnityShader�p
// �ϊ����p�̊֐����܂Ƃ߂��t�@�C��
//	

#include "UnityCG.cginc"

// -------------------------------------------------
// ��]
// -------------------------------------------------
float3 rotate(float3 p, float angle, float3 axis) 
{
	float3 a = normalize(axis);
	float s = sin(angle);
	float c = cos(angle);
	float r = 1.0 - c;
	float3x3 m = float3x3(
		a.x * a.x * r + c,
		a.y * a.x * r + a.z * s,
		a.z * a.x * r - a.y * s,
		a.x * a.y * r - a.z * s,
		a.y * a.y * r + c,
		a.z * a.y * r + a.x * s,
		a.x * a.z * r + a.y * s,
		a.y * a.z * r - a.x * s,
		a.z * a.z * r + c
		);
	return mul(p, m);
}

// -------------------------------------------------
// �g��E�k��
// -------------------------------------------------
float3 scale(float3 p, float3 s)
{
	float3x3 m = float3x3(
		s.x, 0, 0,
		0, s.y, 0,
		0, 0, s.z
		);
		return mul(p, m);
}			

// -------------------------------------------------
// ���s�ړ�
// -------------------------------------------------
float4 trans(float3 p, float3 t)
{
	float4x4 m = float4x4(
		1, 0, 0, t.x,
		0, 1, 0, t.y,
		0, 0, 1, t.z,
		0, 0, 0, 1
		);
		return mul(p, m);
}		
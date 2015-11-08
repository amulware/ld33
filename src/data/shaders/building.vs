#version 130

uniform mat4 projectionMatrix;
uniform mat4 modelviewMatrix;

in vec3 v_position;
in vec3 v_normal;
in vec2 v_uv;
in float v_alpha;

out vec3 p_position;
out vec3 p_normal;
out vec2 p_uv;
out float p_alpha;


void main()
{
	gl_Position = projectionMatrix * modelviewMatrix * vec4(v_position, 1.0);
	p_position = v_position;
	p_normal = v_normal;
	p_uv = v_uv;
	p_alpha = v_alpha;
}
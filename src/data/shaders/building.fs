#version 150

in vec3 p_position;
in vec3 p_normal;
in vec2 p_uv;
in float p_alpha;

out vec4 fragColor;

void main()
{
	float gradient = 1 - 0.5 / (p_position.z * 0.2 + 1);

	vec3 sun = normalize(vec3(1, 0.3, 2));

	float lum = max(0, dot(p_normal, sun));

	vec3 rgb = vec3(1, 1, 0.7) * lum + vec3(0.3, 0.25, 0.4);


    fragColor = vec4(rgb * gradient, 1) * p_alpha;
}
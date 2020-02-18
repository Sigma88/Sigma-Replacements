using UnityEngine;


namespace SigmaReplacements
{
    namespace MenuScenes
    {
        internal class MenuLight : Info
        {
            // Settings
            internal string template = null;
            internal bool enabled = true;


            // Physical Parameters
            internal Vector3? position = null;
            internal Quaternion? rotation = null;
            internal Vector3? scale = null;


            // Visual Parameters
            internal Color? color = null;
            internal float? colorTemperature = null;

            internal float? intensity = null;
            internal float? bounceIntensity = null;

            internal float? range = null;
            internal float? spotAngle = null;
            internal float? shadowStrength = null;

            // Movements
            internal string track = null;
            internal bool invert = false;


            // New MenuLight from cfg
            internal MenuLight(ConfigNode node)
            {
                // Settings
                name = node.GetValue("name");
                template = node.GetValue("template");

                if (!bool.TryParse(node.GetValue("enabled"), out enabled))
                {
                    enabled = true;
                }

                useChance = Parse(node.GetValue("useChance"), useChance);

                if (Time.frameCount % 100 >= useChance * 100)
                {
                    enabled = false;
                }

                // Physical Parameters
                position = Parse(node.GetValue("position"), position);
                rotation = Parse(node.GetValue("rotation"), rotation);
                scale = Parse(node.GetValue("scale"), scale);


                // Visual Parameters
                color = Parse(node.GetValue("color"), color);
                colorTemperature = Parse(node.GetValue("colorTemperature"), colorTemperature);

                intensity = Parse(node.GetValue("intensity"), intensity);
                bounceIntensity = Parse(node.GetValue("bounceIntensity"), bounceIntensity);
                range = Parse(node.GetValue("range"), range);
                spotAngle = Parse(node.GetValue("spotAngle"), spotAngle);
                shadowStrength = Parse(node.GetValue("shadowStrength"), shadowStrength);


                // Movements
                track = node.GetValue("track");
                invert = Parse(node.GetValue("invert"), invert);
            }

            // New MenuLight from name
            internal MenuLight(string name, bool enabled = true)
            {
                // Name
                this.name = name;
                this.enabled = enabled;
            }


            // Apply MenuLight to GameObject
            internal void ApplyTo(GameObject obj, GameObject scene)
            {
                if (obj == null) return;
                if (Debug.debug) obj.AddOrGetComponent<LiveDebug>();

                // Edit Position/Rotation/Scale
                obj.transform.position = position ?? obj.transform.position;
                obj.transform.rotation = rotation ?? obj.transform.rotation;
                obj.transform.localScale = scale ?? obj.transform.localScale;

                // Edit Appearances
                if (obj.GetComponent<Light>() is Light light)
                {
                    light.color = color ?? light.color;
                    light.colorTemperature = colorTemperature ?? light.colorTemperature;
                    light.intensity = intensity ?? light.intensity;
                    light.bounceIntensity = bounceIntensity ?? light.bounceIntensity;
                    light.range = range ?? light.range;
                    light.spotAngle = spotAngle ?? light.spotAngle;
                    light.shadowStrength = shadowStrength ?? light.shadowStrength;
                }

                if (track == "Camera")
                {
                    LightTracker LT = obj.AddOrGetComponent<LightTracker>();
                    LT.target = Camera.main.transform;
                    LT.invert = invert;
                }
                else if (scene?.GetChild(track) is GameObject target)
                {
                    obj.AddOrGetComponent<LightTracker>().target = target.transform;
                }
            }
        }
    }
}

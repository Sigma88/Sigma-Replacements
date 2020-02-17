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
            internal float? rotatoSpeed = null;

            internal string pivotAround = null;
            internal Vector3? pivotPosition = null;
            internal Quaternion? pivotRotation = null;
            internal Vector3? pivotScale = null;
            internal float? pivotDistance = null;
            internal float? pivotRotatoSpeed = null;

            internal float? bobberSeed = null;
            internal Vector3? bobberOFS = null;


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
                rotatoSpeed = Parse(node.GetValue("rotatoSpeed"), rotatoSpeed);

                pivotAround = node.GetValue("pivotAround");
                pivotPosition = Parse(node.GetValue("pivotPosition"), pivotPosition);
                pivotRotation = Parse(node.GetValue("pivotRotation"), pivotRotation);
                pivotScale = Parse(node.GetValue("pivotScale"), pivotScale);
                pivotDistance = Parse(node.GetValue("pivotDistance"), pivotDistance);
                pivotRotatoSpeed = Parse(node.GetValue("pivotRotatoSpeed"), pivotRotatoSpeed);

                bobberSeed = Parse(node.GetValue("bobberSeed"), bobberSeed);
                bobberOFS = Parse(node.GetValue("bobberOFS"), bobberOFS);
            }

            // New MenuLight from name
            internal MenuLight(string name, bool enabled = true)
            {
                // Name
                this.name = name;
                this.enabled = enabled;
            }


            // Apply MenuLight to GameObject
            internal void ApplyTo(GameObject obj, float scaleMult = 1)
            {
                if (obj == null) return;
                if (Debug.debug) obj.AddOrGetComponent<LiveDebug>();

                // Edit Position/Rotation/Scale
                obj.transform.position = position ?? obj.transform.position;
                obj.transform.rotation = rotation ?? obj.transform.rotation;
                if (scale != null)
                    obj.transform.localScale = (Vector3)scale * scaleMult;

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


                // Rotato
                if (rotatoSpeed != null)
                {
                    Rotato rotato = obj.AddOrGetComponent<Rotato>();
                    rotato.speed = (float)rotatoSpeed;
                }


                // Pivot
                if (!string.IsNullOrEmpty(pivotAround))
                {
                    GameObject scene = obj;

                    while (scene != null && scene.name != "OrbitScene" && scene.name != "MunScene")
                    {
                        scene = scene?.transform?.parent?.gameObject;
                    }

                    if (scene == null) return;

                    GameObject parent = null;

                    if (pivotAround != null)
                        parent = scene.GetChild(pivotAround);

                    parent = parent ?? obj?.transform?.parent?.gameObject;


                    if (parent != null)
                    {
                        GameObject pivot = new GameObject(obj.name + "_Pivot");

                        pivot.transform.SetParent(parent.transform);

                        pivot.transform.position = pivotPosition ?? parent.transform.position;
                        pivot.transform.localRotation = pivotRotation ?? Quaternion.Euler(Vector3.zero);
                        pivot.transform.localScale = pivotScale ?? Vector3.one;

                        obj.transform.SetParent(pivot.transform);

                        if (pivotDistance != null)
                        {
                            obj.transform.localPosition = Vector3.left * (float)pivotDistance;
                        }
                    }
                }

                if (pivotRotatoSpeed != null)
                {
                    if (obj?.transform?.parent?.gameObject != null)
                    {
                        Rotato pivotRotato = obj.transform.parent.gameObject.AddOrGetComponent<Rotato>();
                        pivotRotato.speed = (float)pivotRotatoSpeed;
                    }
                }


                // Bobber
                if (bobberSeed != null || bobberOFS != null)
                {
                    Bobber bobber = obj.AddOrGetComponent<Bobber>();

                    bobber.seed = bobberSeed ?? bobber.seed;

                    bobber.ofs1 = bobberOFS?.x ?? bobber.ofs1;
                    bobber.ofs2 = bobberOFS?.y ?? bobber.ofs2;
                    bobber.ofs3 = bobberOFS?.z ?? bobber.ofs3;
                }
            }
        }
    }
}

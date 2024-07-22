using FovChanger;
using SixLabors.ImageSharp.Metadata;
using Swed64;

// init swed
Swed swed = new Swed("cs2");

// get client
IntPtr client = swed.GetModuleBase("client.dll");

// init menu renderer
Renderer renderer = new Renderer();
renderer.Start().Wait();

// get offsets 

// offsets.cs
int dwLocalPlayerPawn = 0x1823A08;

// client.dll.cs

int m_pCameraServices = 0x1130;
int m_iFOV = 0x210;
int m_bIsScoped = 0x22A0;

// fov changer loop
while (true)
{
    uint desiredFov = (uint)renderer.fov;
    // get pawn
    IntPtr localPlayerPawn = swed.ReadPointer(client, dwLocalPlayerPawn);
    // get camera services
    IntPtr cameraServices = swed.ReadPointer(localPlayerPawn, m_pCameraServices);
    // current FOV
    uint currentFov = swed.ReadUInt(cameraServices + m_iFOV);
    // if scoped, we dont write
    bool isScoped = swed.ReadBool(localPlayerPawn, m_bIsScoped);

    if (!isScoped && currentFov != desiredFov) // if we dont have desired fov we change
    {
        swed.WriteUInt(cameraServices + m_iFOV, desiredFov); //  write new fov
    }

    // Thread.Sleep (1); // <-- can add thread sleep, but FOV will lag more         
}

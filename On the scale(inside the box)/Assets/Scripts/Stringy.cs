using UnityEngine;

public class Stringy : MonoBehaviour
{
    public Transform BeamLeft;
    public Transform BeamRight;

    public Transform PlatAleft;
    public Transform PlatAright;
    public Transform PlatAcentre;

    public Transform PlatBleft;
    public Transform PlatBright;
    public Transform PlatBcentre;

    public LineRenderer A;
    public LineRenderer B;
    public LineRenderer C;
    public LineRenderer D;
    public LineRenderer E;
    public LineRenderer F;

    private void Update()
    {
        A.SetPosition(0,BeamLeft.position);
        A.SetPosition(1,PlatAleft.position);

        B.SetPosition(0, BeamLeft.position);
        B.SetPosition (1,PlatAright.position);

        C.SetPosition(0, BeamLeft.position);
        C.SetPosition (1,PlatAcentre.position);

        D.SetPosition(0, BeamRight.position);
        D.SetPosition (1,PlatBleft.position);

        E.SetPosition (0, BeamRight.position);
        E.SetPosition(1,PlatBcentre.position);

        F.SetPosition (0, BeamRight.position);
        F.SetPosition(1,PlatBright.position);
    }

}

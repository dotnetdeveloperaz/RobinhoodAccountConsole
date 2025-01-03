using System.Text;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Crypto;
using RobinhoodAccountConsole.Models;

namespace RobinhoodAccountConsole;

internal class Signature
{
    private string _method;
    private string _path;
    private string? _body = null;
    private long _timestamp;
    private bool _isSignatureValid = false;
    private ApiServer _apiServer;
    private string? _base64Signature;
    private bool _hasBody = false;

    public long Timestamp { get { return _timestamp; } } 
    public bool IsSignatureValid { get { return _isSignatureValid; } }
    public string? Base64Signature { get { return _base64Signature; } }
    public string? Body { get { return _body; } }
    public bool HasBody { get { return _hasBody; } }

    /// <summary>
    /// Creates the signature object to create the signature for the request without a body
    /// </summary>
    /// <param name="apiserver"></param>
    /// <param name="method"></param>
    /// <param name="path"></param>
    /// 
    public Signature(ApiServer apiserver, string method, string path)
    {
        _apiServer = apiserver;
        _method = method;
        _path = path;
    }
    /// <summary>
    /// Creates the signature object to create the signature for the request with body
    /// </summary>
    /// <param name="apiserver"></param>
    /// <param name="method"></param>
    /// <param name="path"></param>
    /// <param name="body"></param>
    public Signature(ApiServer apiserver, string method, string path, string body)
    {
        _apiServer = apiserver;
        _method = method;
        _path = path;
        _body = body;
        _hasBody = true;
    }

    /// <summary>
    /// Signs the message for the API call.
    /// </summary>
    /// <returns></returns>
    internal bool Sign()
    {
        // Convert base64 strings to bytes
        byte[] privateKeyBytes = Convert.FromBase64String(_apiServer.PrivateKey);
        byte[] publicKeyBytes = Convert.FromBase64String(_apiServer.PublicKey);

        // Create private key and public key
        Ed25519PrivateKeyParameters edPrivateKey = new Ed25519PrivateKeyParameters(privateKeyBytes, 0);
        Ed25519PublicKeyParameters edPublicKey = new Ed25519PublicKeyParameters(publicKeyBytes, 0);

        // Create the message to sign
        _timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        string message;
        if (_body != null)
            message = $"{_apiServer.ApiKey}{_timestamp}{_path}{_method}{_body}";
        else
            message = $"{_apiServer.ApiKey}{_timestamp}{_path}{_method}";

            // Sign the message
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
        ISigner signer = new Ed25519Signer();
        signer.Init(true, edPrivateKey);
        signer.BlockUpdate(messageBytes, 0, messageBytes.Length);
        byte[] signature = signer.GenerateSignature();
        _base64Signature = Convert.ToBase64String(signature);

        // Verify the signature
        ISigner verifier = new Ed25519Signer();
        verifier.Init(false, edPublicKey);
        verifier.BlockUpdate(messageBytes, 0, messageBytes.Length);
        _isSignatureValid = verifier.VerifySignature(signature);

        return _isSignatureValid;
    }
}

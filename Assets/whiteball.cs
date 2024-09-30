using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BolaPutih : MonoBehaviour
{
    private Rigidbody rb;
    private bool isAiming = false; // Mengecek apakah pemain sedang mengarahkan bola

    // Posisi awal bola
    private Vector3 posisiAwal;

    // Kekuatan tembakan bola
    public float kekuatanTembak = 10f;

    // Batas bawah untuk mengembalikan bola
    public float batasBawah = -5f; // Bola tidak boleh jatuh di bawah -5 di sumbu Y

    // LineRenderer untuk menggambar garis bantu
    private LineRenderer lineRenderer;

    // Panjang garis bantu
    public float maxLineLength = 5f;

    void Start()
    {
        // Mengambil komponen Rigidbody pada bola
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody tidak ditemukan! Tambahkan komponen Rigidbody ke objek ini.");
        }

        // Simpan posisi awal bola saat permainan dimulai
        posisiAwal = transform.position;

        // Setup LineRenderer untuk garis bantu
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 2; // Garis bantu dengan dua titik
        lineRenderer.startWidth = 0.05f; // Ketebalan garis awal
        lineRenderer.endWidth = 0.05f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
        lineRenderer.enabled = false; // Nonaktifkan pada awalnya
    }

    void Update()
    {
        // Cek apakah bola jatuh di bawah batas yang ditentukan
        if (transform.position.y < batasBawah)
        {
            // Reset bola ke posisi awal
            ResetBolaKePosisiAwal();
        }

        // Jika sedang mengarahkan tembakan, perbarui garis bantu
        if (isAiming)
        {
            UpdateGarisBantu();
        }
    }

    // Fungsi ini dipanggil saat mouse klik pada bola (mulai mengarahkan tembakan)
    void OnMouseDown()
    {
        isAiming = true;
        rb.isKinematic = true; // Menghentikan bola dari pengaruh fisika saat mengarahkan
        lineRenderer.enabled = true; // Aktifkan garis bantu
    }

    // Fungsi ini dipanggil saat mouse dilepas (meluncurkan bola)
    void OnMouseUp()
    {
        isAiming = false;
        rb.isKinematic = false; // Bola kembali diatur oleh fisika setelah tembakan
        lineRenderer.enabled = false; // Nonaktifkan garis bantu setelah tembakan

        // Meluncurkan bola ke arah yang ditentukan oleh garis bantu
        LaunchBall();
    }

    // Fungsi untuk mengarahkan bola mengikuti posisi mouse
    void UpdateGarisBantu()
    {
        // Mendapatkan posisi mouse di layar dan ubah menjadi posisi dunia
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.WorldToScreenPoint(transform.position).z; // Sesuaikan dengan jarak Z bola

        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // Hitung arah tembakan dari bola ke posisi mouse
        Vector3 arahTembakan = (worldMousePosition - transform.position).normalized;

        // Tentukan panjang garis bantu berdasarkan arah tembakan
        Vector3 endPosition = transform.position + arahTembakan * maxLineLength;

        // Atur posisi awal dan akhir dari garis bantu
        lineRenderer.SetPosition(0, transform.position); // Titik awal (posisi bola)
        lineRenderer.SetPosition(1, endPosition); // Titik akhir (arah tembakan)
    }

    // Fungsi untuk meluncurkan bola ke arah yang ditentukan oleh garis bantu
    void LaunchBall()
    {
        // Arah tembakan adalah arah yang sama dengan garis bantu
        Vector3 arahTembakan = (lineRenderer.GetPosition(1) - lineRenderer.GetPosition(0)).normalized;

        // Berikan gaya ke bola berdasarkan arah tembakan dan kekuatan tembakan
        rb.AddForce(arahTembakan * kekuatanTembak, ForceMode.Impulse);
    }

    // Fungsi untuk mengembalikan bola ke posisi awal jika jatuh
    void ResetBolaKePosisiAwal()
    {
        // Reset posisi bola ke posisi awal
        transform.position = posisiAwal;

        // Reset kecepatan bola agar tidak bergerak setelah reset
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        Debug.Log("Bola kembali ke posisi awal.");
    }
}

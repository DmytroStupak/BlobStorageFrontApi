import { useState } from "react";
import validator from "validator";

export default function Home() {
  const [resp, setResp] = useState("");
  const [error, setError] = useState("");
  const [files, setFiles] = useState([]);
  const [email, setEmail] = useState("");
  const [msgEmail, setMsgEmail] = useState("");
  const [inputKey, setInputKey] = useState(0);

  function onChangeFile(event) {
    event.preventDefault();
    setResp("");
    if (validateFile(event.target.files[0])) {
      setFiles(event.target.files);
      setError("");
    } else {
      setError("Invalid extension");
      setFiles([]);
    }
  }

  function onChangeEmail(event) {
    event.preventDefault();
    setEmail(event.target.value);
  }

  function validateFile(file) {
    const fileExtension = file.name.split(".").pop();
    const res = ["docx"].includes(fileExtension.toLowerCase()) ? true : false;
    return res;
  }

  function postFile(event) {
    setResp("");

    if (validator.isEmail(email)) {
      setMsgEmail("");
    } else {
      setMsgEmail("Invalid email adress");
      return;
    }

    if (files.length >= 1) {
      event.preventDefault();

      const formData = new FormData();
      formData.append("file", files[0], `${email},${files[0].name}`);

      fetch(
        "https://reenbittask20240309172531.azurewebsites.net//blobservice",
        {
          method: "POST",
          body: formData,
        }
      )
        .then((res) => res.json())
        .then((data) =>
          !data.error
            ? setResp(`File "${files[0].name}" uploaded successfully`)
            : setResp("")
        )
        .then(() => setFiles([]))
        .catch((err) => console.log(err));
      setInputKey(inputKey + 1);
    }
  }

  return (
    <div>
      {error && (
        <div>
          <span style={{ color: "red" }}>{error}</span>
        </div>
      )}

      {msgEmail && (
        <div>
          <span style={{ color: "red" }}>{msgEmail}</span>
        </div>
      )}

      <div class="form-group">
        <label for="exampleFormControlInput1">Email address</label>
        <input
          type="email"
          class="form-control"
          id="exampleFormControlInput1"
          key={inputKey}
          placeholder="name@example.com"
          style={{ width: 400 }}
          onChange={onChangeEmail}
        />
      </div>
      <label class="form-label" for="customFile">
        Select a file with the extension ".docx":
      </label>
      <input
        accept=".docx"
        type="file"
        class="form-control"
        key={inputKey}
        id="customFile"
        onChange={onChangeFile}
        style={{ width: 400 }}
      />
      {resp && (
        <div>
          <span style={{ fontWeight: 600, marginLeft: 10 }}>{resp}</span>
        </div>
      )}
      <div class="d-grid gap-2 d-md-flex justify-content-md-start mt-2">
        <button
          class="btn btn-primary"
          type="button"
          value="Upload"
          data-mdb-ripple-init
          onClick={postFile}
          style={{ marginTop: 2 }}
        >
          Upload
        </button>
      </div>
    </div>
  );
}

import React, { useState, useEffect } from "react";
import "./candidates.scss";
import {
  ICompany,
  ICreateCandidateDto,
  ICreateJobDto,
  IJob,
} from "../../types/global.typing";
import {
  Button,
  FormControl,
  InputLabel,
  MenuItem,
  Select,
  TextField,
} from "@mui/material";
import { useNavigate } from "react-router-dom";
import httpModule from "../../helpers/http.module";

const AddCandidate = () => {
  const [candidate, setcandidate] = useState<ICreateCandidateDto>({
    firstName: "",
    lastName: "",
    email: "",
    phone: "",
    coverLetter: "",
    jobId: "",
  });
  const [jobs, setJobs] = useState<IJob[]>([]);
  const [pdfFile, setPdfFile] = useState<File | null>();

  const redirect = useNavigate();

  useEffect(() => {
    httpModule
      .get<IJob[]>("/Job/Get")
      .then((response) => {
        setJobs(response.data);
      })
      .catch((error) => {
        alert("Error");
        console.log(error);
      });
  }, []);

  const handleClickSaveBtn = () => {
    if (
      candidate.firstName === "" ||
      candidate.lastName === "" ||
      candidate.email === "" ||
      candidate.phone === "" ||
      candidate.jobId === "" ||
      !pdfFile
    ) {
      alert("Fill all fields");
      return;
    }
    const newCandidateFormData = new FormData();
    newCandidateFormData.append("firstName", candidate.firstName);
    newCandidateFormData.append("lastName", candidate.lastName);
    newCandidateFormData.append("email", candidate.email);
    newCandidateFormData.append("phone", candidate.phone);
    newCandidateFormData.append("coverLetter", candidate.coverLetter);
    newCandidateFormData.append("jobId", candidate.jobId);
    newCandidateFormData.append("pdfFile", pdfFile);

    httpModule
      .post("/Candidate/Create", newCandidateFormData)
      .then((response) => redirect("/candidates"))
      .catch((error) => console.log(error));
  };

  const handleClickBackBtn = () => {
    redirect("/candidates");
  };

  return (
    <div className="content">
      <div className="add-candidate">
        <h2>Add New Candidate</h2>

        <FormControl fullWidth>
          <InputLabel>Job</InputLabel>
          <Select
            value={candidate.jobId}
            label="Job"
            onChange={(e) =>
              setcandidate({ ...candidate, jobId: e.target.value })
            }
          >
            {jobs.map((item) => (
              <MenuItem key={item.id} value={item.id}>
                {item.title}
              </MenuItem>
            ))}
          </Select>
        </FormControl>

        <TextField
          autoComplete="off"
          label="First Name"
          variant="outlined"
          value={candidate.firstName}
          onChange={(e) =>
            setcandidate({ ...candidate, firstName: e.target.value })
          }
        />
        <TextField
          autoComplete="off"
          label="Last Name"
          variant="outlined"
          value={candidate.lastName}
          onChange={(e) =>
            setcandidate({ ...candidate, lastName: e.target.value })
          }
        />
        <TextField
          autoComplete="off"
          label="Email"
          variant="outlined"
          value={candidate.email}
          onChange={(e) =>
            setcandidate({ ...candidate, email: e.target.value })
          }
        />
        <TextField
          autoComplete="off"
          label="Phone"
          variant="outlined"
          value={candidate.phone}
          onChange={(e) =>
            setcandidate({ ...candidate, phone: e.target.value })
          }
        />
        <TextField
          autoComplete="off"
          label="Cover Letter"
          variant="outlined"
          rows={4}
          multiline={true}
          value={candidate.coverLetter}
          onChange={(e) =>
            setcandidate({ ...candidate, coverLetter: e.target.value })
          }
        />

        <input
          type="file"
          onChange={(event) =>
            setPdfFile(event.target.files ? event.target.files[0] : null)
          }
        />

        <div className="btns">
          <Button
            variant="outlined"
            color="primary"
            onClick={handleClickSaveBtn}
          >
            Save
          </Button>
          <Button
            variant="outlined"
            color="secondary"
            onClick={handleClickBackBtn}
          >
            Back
          </Button>
        </div>
      </div>
    </div>
  );
};

export default AddCandidate;

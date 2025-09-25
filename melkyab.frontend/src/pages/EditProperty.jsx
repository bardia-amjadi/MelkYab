import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import PropertyForm from "../components/PropertyForm";

export default function EditProperty() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [property, setProperty] = useState(null);

  useEffect(() => {
    fetch(`http://localhost:8000/api/v1/properties/${id}`)
      .then(res => res.json())
      .then(data => setProperty(data.property));
  }, [id]);

  const handleSubmit = async (data) => {
    await fetch(`http://localhost:8000/api/v1/properties/${id}`, {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ ...data, id }),
    });
    navigate("/properties");
  };

  if (!property) return <p className="p-6">در حال بارگذاری...</p>;

  return (
    <div className="p-6">
      <h1 className="text-xl font-bold mb-4">ویرایش ملک</h1>
      <PropertyForm initialData={property} onSubmit={handleSubmit} />
    </div>
  );
}

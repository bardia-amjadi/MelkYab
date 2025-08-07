# 📋 طراحی دیتابیس رزرو اقامتگاه
نسخه اولیه

---

## 🧑‍💼 جدول کاربران (Users)

| نام فیلد       | نوع      | توضیح فارسی         |
| --------------- | -------- | ------------------- |
| id              | VARCHAR  | شناسه یکتا          |
| email           | VARCHAR  | ایمیل کاربر         |
| password_hash   | VARCHAR  | رمز عبور هش‌شده     |
| fullname        | NVARCHAR | نام و نام خانوادگی  |
| phone           | VARCHAR  | شماره تماس          |
| role            | VARCHAR  | نقش (admin یا user) |
| created_at      | DATE     | تاریخ ورود          |

---

## 🏠 جدول اقامتگاه‌ها (Properties)

| نام فیلد           | نوع      | توضیح فارسی         |
| ------------------- | -------- | ------------------- |
| id                  | VARCHAR  | شناسه یکتا          |
| title               | NVARCHAR | عنوان آگهی          |
| description         | NVARCHAR | توضیحات کامل        |
| type                | VARCHAR  | نوع (ویلا، سوئیت...) |
| max_guests          | INT      | حداکثر نفرات        |
| bedrooms            | INT      | تعداد اتاق          |
| beds                | INT      | تعداد تخت           |
| bathrooms           | INT      | تعداد حمام          |
| price_per_night     | DECIMAL  | قیمت هر شب          |
| address             | NVARCHAR | آدرس                |
| is_active           | BOOLEAN  | فعال / غیرفعال      |
| created_by_user_id  | VARCHAR  | ایجاد شده توسط      |
| created_at          | DATE     | تاریخ ثبت           |

---

## 🖼️ جدول تصاویر اقامتگاه (PropertyImages)

| فیلد        | نوع     | توضیح         |
| ----------- | ------- | ------------- |
| id          | VARCHAR | شناسه تصویر   |
| property_id | VARCHAR | ارجاع به اقامتگاه |
| image_url   | VARCHAR | آدرس تصویر    |
| is_cover    | BOOLEAN | تصویر اصلی    |

---

## ✨ جدول امکانات (Amenities)

| فیلد | نوع      | توضیح  |
| ---- | -------- | ------- |
| id   | VARCHAR  | شناسه   |
| name | NVARCHAR | نام امکان |

---

## 🔗 جدول اتصال امکانات به اقامتگاه (PropertyAmenities)

| فیلد        | نوع     | توضیح     |
| ----------- | ------- | ---------- |
| id          | VARCHAR | شناسه      |
| property_id | VARCHAR | اقامتگاه   |
| amenity_id  | VARCHAR | امکان      |

---

## 📅 جدول رزروها (Bookings)

| فیلد           | نوع      | توضیح           |
| --------------- | -------- | ---------------- |
| id              | VARCHAR  | شناسه رزرو       |
| user_id         | VARCHAR  | کاربر رزرو‌کننده |
| property_id     | VARCHAR  | اقامتگاه         |
| check_in_date   | DATE     | تاریخ ورود       |
| check_out_date  | DATE     | تاریخ خروج       |
| guests          | INT      | تعداد مهمانان    |
| total_price     | DECIMAL  | قیمت کل          |
| status          | NVARCHAR | وضعیت رزرو       |
| created_at      | DATE     | تاریخ ایجاد      |

---

> ساختار جدول دیتابیس

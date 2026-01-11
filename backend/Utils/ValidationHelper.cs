using System;
using System.Text.RegularExpressions;

namespace ClinicManagement.Utils 
{
    public static class ValidationHelper
    {
        // Định nghĩa Regex Pattern một lần để dùng chung (Tối ưu hiệu năng)
        // Regex tên tiếng Việt đầy đủ
        private static readonly string NamePattern = @"^[a-zA-ZÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỂẾưăạảấầẩẫậắằẳẵặẹẻẽềểếỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵỷỹ\s]+$";
        // Regex số điện thoại VN (03, 05, 07, 08, 09)
        private static readonly string PhonePattern = @"^(0[3|5|7|8|9])[0-9]{8}$";
        // Regex Email cơ bản
        private static readonly string EmailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        // Regex Mã BHYT (10 chữ số)
        private static readonly string InsuranceCodePattern = @"^\d{10}$";

        /// <summary>
        /// Kiểm tra Họ và Tên
        /// </summary>
        public static string ValidateFullName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return "Họ tên không được để trống.";

            name = name.Trim();

            if (name.Length < 2)
                return "Họ tên phải có ít nhất 2 ký tự.";

            if (!Regex.IsMatch(name, NamePattern))
                return "Họ tên chỉ được chứa chữ cái và khoảng trắng.";

            return null; // Hợp lệ
        }

        /// <summary>
        /// Kiểm tra Số điện thoại
        /// </summary>
        public static string ValidatePhoneNumber(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return "Số điện thoại không được để trống.";

            phone = phone.Trim();

            if (!Regex.IsMatch(phone, PhonePattern))
                return "SĐT không đúng định dạng (VD: 0901234567).";

            return null; // Hợp lệ
        }

        /// <summary>
        /// Kiểm tra Email (Cho phép null/rỗng - nếu nhập thì phải đúng)
        /// </summary>
        public static string ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return null; // Email không bắt buộc

            if (!Regex.IsMatch(email.Trim(), EmailPattern))
                return "Email không đúng định dạng.";

            return null;
        }

        /// <summary>
        /// Kiểm tra Ngày sinh
        /// </summary>
        public static string ValidateBirthDate(DateTime? birthDate)
        {
            if (!birthDate.HasValue) return null; // Ngày sinh không bắt buộc (tùy nghiệp vụ)

            if (birthDate.Value > DateTime.Now)
                return "Ngày sinh không được lớn hơn ngày hiện tại.";

            if (birthDate.Value.Year < 1900)
                return "Năm sinh không hợp lệ.";

            return null;
        }

        /// <summary>
        /// Kiểm tra Mã Bảo Hiểm Y Tế
        /// </summary>
        public static string ValidateInsuranceCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code)) return null; // Không bắt buộc

            if (!Regex.IsMatch(code.Trim(), InsuranceCodePattern))
                return "Mã BHYT phải có đúng 10 chữ số.";

            return null;
        }
    }
}
using System;
using Moonlit.Mvc;

namespace MyTestRule
{
    public class FromPersonModel : IFromEntity<PersonEntity>
    {
        [Mapping(To = "FirstName")]
        public string FirstName { get; set; }
        [Mapping( )]
        public string LastName { get; set; }
        [Mapping()]
        public string MiddleName { get; set; }

        #region Implementation of IFromEntity<PersonEntity>

        public void OnFromEntity(PersonEntity entity, FromEntityContext context)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
    public class ToPersonModel : IToEntity<PersonEntity>
    {
        [Mapping(To = "FirstName")]
        public string FirstName { get; set; }
        [Mapping( )]
        public string LastName { get; set; }
        [Mapping()]
        public string MiddleName { get; set; }

        #region Implementation of IFromEntity<PersonEntity>

        public void OnFromEntity(PersonEntity entity, FromEntityContext context)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Implementation of IToEntity<PersonEntity>

        public void OnToEntity(PersonEntity entity, ToEntityContext context)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class User 
    {
        public int UserId { get; set; }




        public string UserName { get; set; }


        public string LoginName { get; set; }


        public string Password { get; set; }

        public string Gender { get; set; }



        public DateTime? DateOfBirth { get; set; }



        public bool IsEnabled { get; set; }

        public int CultureId { get; set; }




        public bool IsSuper { get; set; }


        public virtual int Roles { get; set; }


        public bool IsBuildIn { get; set; }

        public virtual int LoginFailedLogs { get; set; }
        public string Avatar { get; set; }
       

     
    }
    public class AdminUserCreateModel : IFromEntity<User>, IToEntity<User>
    {
        
        [Field(FieldWidth.W4)]
        [Mapping]
        public string UserName { get; set; }

        [Field(FieldWidth.W4)]
        [Mapping]
        public string LoginName { get; set; }
 
        [Field(FieldWidth.W4)]
        [PasswordBox]
        public string Password { get; set; }

        [Field(FieldWidth.W4)]
        [Mapping]
        public int Gender { get; set; }
 
        [Field(FieldWidth.W4)]
        [Mapping]
        public DateTime? DateOfBirth { get; set; }

        [Mapping]
        public int CultureId { get; set; }
        [Field(FieldWidth.W4)]
        [Mapping(OnlyNotPostback = false)]
        public bool IsSuper { get; set; }

        public int[] Roles { get; set; }

    
        [Field(FieldWidth.W4)]
        [Mapping]
        public bool IsEnabled { get; set; }



        public void OnFromEntity(User entity, FromEntityContext context)
        {
        }

        public void OnToEntity(User entity, ToEntityContext context)
        {
            
        }
    }
}